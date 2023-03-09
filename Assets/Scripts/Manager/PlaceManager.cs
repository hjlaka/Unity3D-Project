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

    [SerializeField]
    private PlacementRememberer placeRememberer;

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
    private InfluenceCalculator influenceCalculator;
    //=================================

    private void Awake()
    {
        influenceCalculator = GetComponentInChildren<InfluenceCalculator>();
    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        SelectedPiece.ChangeColor(selectingColor);
        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
            GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PLACE);

        // ����
        OnSelectPiece?.Invoke(SelectedPiece);
        OnLeaveSelect.AddListener(SelectedPiece.ChangeColor);
        //TODO: �̵� ���� ���� ������ ������ �ϳ��� ��� ������
    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;
        // ���õ� �⹰�� �̵����� �ʰ� �ٷ� ����ϴ� ���

        //����
        IMarkable markable = selectedPiece.place.board as IMarkable;
        if (markable != null)
            markable.PreShowEnd(selectedPiece);

        SelectedPieceInit();
        GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PIECE);
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

    public void ShowOnBoard(Piece piece)
    {
        // � ���忡�� ������ �Ǿ�� ���� ã�� ����
        IMarkable markable = piece.place.board as IMarkable;
        if (markable != null)
            markable.PreShow(piece);
    }

    private bool IsPlaceable(Place place, Piece piece)
    {
        if (place.board == null) 
            return true;
        if (place.board != piece.place.board) 
            return true;
        if (!place.board.FollowRule) 
            return true;

        return place.IsMovableToCurPiece;
    }


    public void MoveProcess(Piece piece, Place place)
    {
        Place oldPlace = piece.place;
        IMarkable oldBoard = oldPlace.board as IMarkable;
        IMarkable newBoard = place.board as IMarkable;
        Placement subsequent = null;


        // ������ �� �ִ� ������ Ȯ��
        if (!IsPlaceable(place, piece))
        {
            Debug.Log("������ �� ���� ��");
            return;
        }

        if(GameManager.Instance.turnState == GameManager.TurnState.BOTTOM_TURN)
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.DOING_PLAYER_TURN_START);
        }
        else if(GameManager.Instance.turnState == GameManager.TurnState.TOP_TURN)
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.OPPONENT_TURN_START);
        }
                
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
        DialogueManager.Instance.CheckDialogueEvent();

        // �̺�Ʈ ���� Ȯ�� �� �� ����
        StartCoroutine(EndTurn(piece, true));


        // �޸��� ���
        if(oldBoard != null && oldBoard == newBoard)
        {
            Placement newPlacement = new Placement(piece, oldPlace, place, attackedPiece, subsequent);
            // �޸��並 ���⼭ �����ؾ� �ұ�?
            placeRememberer.SaveMemento(newPlacement);
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

    private IEnumerator EndTurn(Piece endedPiece, bool endMark)
    {
        // �⺻ ��� �ð�
        yield return new WaitForSeconds(1.5f);

        while (GameManager.Instance.state == GameManager.GameState.IN_CONVERSATION)
        {
            yield return null;

            // ���ʰ� �����ٴ� ��ȣ�� ������ ��ٸ���.
        }
        Debug.Log("���� ���� ���� �Ǿ�����");

        SelectedPieceInit();

        if(endMark)
        {
            IMarkable markable = endedPiece.place.board as IMarkable;
            if (markable != null)
            {
                markable.PostShowEnd(endedPiece);
                Debug.Log("ǥ�� ����");
            }
        }

        GameManager.Instance.ChangeGameState(GameManager.GameState.TURN_FINISHED);

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
