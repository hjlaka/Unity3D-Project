using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceManager : SingleTon<PlaceManager>
{
    [SerializeField]
    private Piece selectedPiece;
    public Piece SelectedPiece { get { return selectedPiece; } set { selectedPiece = value; } }

    // ========== Event ==============
    public UnityEvent<Piece> OnSelectPiece;
    public UnityEvent OnLeaveSelect;
    public UnityEvent OnFinishMove;
    public UnityEvent OnStartMove;
    public UnityEvent OnAttack;
    //================================

    public Place selectedPlace;
    public Place expelZone;

    [Header("Piece")]
    [SerializeField]
    private Color selectingColor;

    [Header("Place")]
    [SerializeField]
    public Color highlight;
    [SerializeField]
    private Color attackable;

    //============ Facade =============
    public InfluenceCalculator influenceCalculator { get; private set; }
    private PlacementRememberer placementRememberer;
    //=================================

    private void Awake()
    {
        influenceCalculator = GetComponentInChildren<InfluenceCalculator>();
        placementRememberer = GetComponentInChildren<PlacementRememberer>();
    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        SelectedPiece.ChangeColor(selectingColor);
/*        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
            GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PLACE);*/

        // ������ �� �ִ� ���� ���
        piece.place?.board?.ApplyMovable(piece, true);

        // ����
        OnSelectPiece?.Invoke(SelectedPiece);
        OnLeaveSelect.AddListener(SelectedPiece.ChangeColor);
        //TODO: �̵� ���� ���� ������ ������ �ϳ��� ��� ������
    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;
        // ���õ� �⹰�� �̵����� �ʰ� �ٷ� ����ϴ� ���

        // ������ �� �ִ� ���� ��� ����
        selectedPiece.place?.board?.ApplyMovable(selectedPiece, false);

        //����
        IMarkable markable = selectedPiece.place.board as IMarkable;
        if (markable != null)
            markable.PreShowEnd(selectedPiece);

        SelectedPieceInit();
        //GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PIECE);
    }

    public void SelectedPieceInit()
    {
        Debug.Log("�⹰ ���� ����");
        //���õ� �⹰�� 

        if (selectedPiece == null)
            return;

        OnLeaveSelect?.Invoke();
        SelectedPiece.ChangeColor();

        SelectedPiece = null;
    }

    public void GetWill(Piece subject, ITargetable target)
    {
        // �� �̺�Ʈ ����

        ITargetable.Type turnType = target.React();

        switch(turnType)
        {
            case ITargetable.Type.Peace:
                break;
            case ITargetable.Type.Attack:
                break;
        }
    }

    public void ShowOnBoard(Piece piece)
    {
        // � ���忡�� ������ �Ǿ�� ���� ã�� ����
        IMarkable markable = piece.place?.board as IMarkable;
        if (markable != null)
            markable.PreShow(piece);
    }

    private bool IsPlaceable(Place place, Piece piece)
    {
        if (place.board == null)
        {
            Debug.Log("���尡 �ƴ� ��ġ");
            return true;
        }

        if (place.board != piece.place.board)
        {
            Debug.Log("���尡 ���� ����� �ٸ� ��ġ");
            return true;
        }
        
        if (!place.board.FollowRule)
        {
            Debug.Log("��Ģ�� ������ �ʴ� ����");
            return true;
        }
        

        return place.IsMovableToCurPiece;
    }


    // �����? �߰��� ����? �ʿ��ұ�? (���ο����� �� ����͸� ��� �ұ�?)
    public void CalculateInfluence(Piece piece)
    {
        influenceCalculator.CalculateInfluence(piece);
    }
    public void ApplyInfluence(Piece piece)
    {
        influenceCalculator.ApplyInfluence(piece);
    }
    public void ReCalculateInfluence(Piece piece)
    {
        influenceCalculator.ReCalculateInfluence(piece);
    }


    public void MoveProcess(Piece piece, Place place)
    {
        Place oldPlace = piece.place;
        IMarkable oldBoard = oldPlace?.board as IMarkable;
        IMarkable newBoard = place.board as IMarkable;
        Placement subsequent = null;


        // ������ �� �ִ� ������ Ȯ��
        if (!IsPlaceable(place, piece))
        {
            Debug.Log("������ �� ���� ��");
            return;
        }

        GameManager.Instance.playerValidToSelectPlace = false;

        // ���� - ����Ʈ ������.
        if (oldBoard != null)
            oldBoard.PreShowEnd(piece);

        // �⹰ �̵�
        Piece attackedPiece = MovePiece(piece, place);

        
        if(oldPlace.MoveAction != null)
        {
            subsequent = oldPlace.MoveAction.DoAction();
        }

        // ���� - ����Ʈ ������ - ����Ʈ�� �޾ƿ� �� ������ ���� ���̴�.
        if (newBoard != null)
        {
            newBoard.PostShow(piece);
        }
        

        // ����
        OnFinishMove?.Invoke();

        // �̺�Ʈ
        DialogueManager.Instance.CheckDialogueEvent(EndTurn);

        // �̺�Ʈ ���� Ȯ�� �� �� ����



        // �޸��� ���
        if(oldBoard != null && oldBoard == newBoard)
        {
            Placement newPlacement = new Placement(piece, oldPlace, place, attackedPiece, subsequent);
            // �޸��並 ���⼭ �����ؾ� �ұ�?
            placementRememberer.SaveMemento(newPlacement);
            Debug.Log("�޸��並 �����ߴ�");
        }
    }

    public Piece MovePiece(Piece piece, Place place)
    {
        if(piece == null || place == null) 
            return null;

        Place oldPlace = piece.place;
        if(oldPlace != null)
            influenceCalculator.InitInfluence(piece);

        // ����
        Piece attackedPiece = piece.SetInPlace(place);    // �⹰�� ��� ��ġ �����

        if (attackedPiece != null)
        {
            Attack(piece, attackedPiece);
        }

        influenceCalculator.CalculateInfluence(piece);
        influenceCalculator.ApplyInfluence(piece);

        // �⹰�� �Ű��� ��, ��ȭ�� �ڸ��� �˸� �߼�
        oldPlace?.notifyObserver();
        place.notifyObserver();

        return attackedPiece;
    }

    private void EndTurn()
    {
        StartCoroutine(EndTurnCoroutine());
    }
    private IEnumerator EndTurnCoroutine()
    {
        Piece endedPiece = selectedPiece;
        // �⺻ ��� �ð�
        yield return new WaitForSeconds(1.5f);

        Debug.Log("���� ���� ���� �Ǿ�����");

        SelectedPieceInit();

        IMarkable markable = endedPiece.place.board as IMarkable;
        if (markable != null)
        {
            markable.PostShowEnd(endedPiece);
            Debug.Log("ǥ�� ����");
        }

        GameManager.Instance.SetNextState(GameManager.GameState.TURN_FINISHED);

        // ī�޶� ����
        OnLeaveSelect?.Invoke();
    }

    public void Attack(Piece piece, Piece target)
    {
        ExpelPiece(target);

        CameraController.Instance.AddToTargetGroup(piece.transform);
        OnAttack?.Invoke();
        CameraController.Instance.RemoveFromTargetGroup(piece.transform);
    }

    public void ExpelPiece(Piece piece)
    {
        influenceCalculator.InitInfluence(piece);
        piece.place = null;
        piece.transform.Translate(new Vector3(0, 0, 0));
        //MovePiece(piece, expelZone);
        piece.IsFree = true;
    }
}
