using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceManager : SingleTon<PlaceManager>
{
    [SerializeField]
    private Piece selectedPiece;
    public Piece SelectedPiece { get { return selectedPiece; } set { selectedPiece = value; /* �� ���� �߰�?*/ } }

    // ========== Event ==============
    public UnityEvent<Piece> OnSelectPiece;
    public UnityEvent OnLeaveSelect;
    public UnityEvent OnFinishAction;
    public UnityEvent<Piece> OnStartAction;
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

    //========== Turn Event ===========
    private TurnEvent turnEventPeace;
    private TurnEvent turnEventAttack;
    //=================================

    //========== Observer =============
    private List<ISubject> changedSubjects;
    //=================================

    private void Awake()
    {
        influenceCalculator = GetComponentInChildren<InfluenceCalculator>();
        placementRememberer = GetComponentInChildren<PlacementRememberer>();

        turnEventPeace = GetComponentInChildren<TurnEventPeace>();
        turnEventAttack = GetComponentInChildren<TurnEventAttack>();

        changedSubjects = new List<ISubject>();
    }

    public void SelectPiece(Piece piece)
    {
        // ���� ����
        if (!GameManager.Instance.TurnActionDecided)
        {
            Debug.Log("���� ��� �ȵ�");
            return; 
        }

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

    public void GetWill(Piece subject, IOnBoardTargetable target)
    {
        Debug.Log("���� ����");
        // ��ȿ�� �˻�

        if (!GameManager.Instance.TurnActionDecided)
        {
            Debug.Log("��ȿ���� ���� ����");
            return;
        }
        if (!CheckTargetValidity(target))
        {
            Debug.Log("�̵� �Ұ��� ��ġ");
            return;
        }
        GameManager.Instance.TurnActionDecided = false;
        OnStartAction?.Invoke(selectedPiece);

        // �� �̺�Ʈ ����

        IOnBoardTargetable.Type turnType = target.React();

        

        switch(turnType)
        {
            case IOnBoardTargetable.Type.Peace:
                turnEventPeace.SetTurnEvent(subject, target);
                turnEventPeace.DoTurn();
                break;
            case IOnBoardTargetable.Type.Attack:
                turnEventAttack.SetTurnEvent(subject, target);
                turnEventAttack.DoTurn();
                break;
        }
    }

    private bool CheckTargetValidity(IOnBoardTargetable targetable)
    {
        Debug.Log(string.Format("{0}�� {1}���� ���� �߻�", selectedPiece, targetable));
        if (targetable is Place)
        {

            return (targetable as Place).IsMovableToCurPiece;
 
        }
        else if (targetable is Piece)
        {
            return (targetable as Piece).place.IsMovableToCurPiece;

        }
        Debug.Log("�̵� �Ұ��� ��ġ");
        return false;
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


    /*public void MoveProcess(Piece piece, Place place)
    {
        Place oldPlace = piece.place;
        IMarkable oldBoard = oldPlace?.board as IMarkable;
        IMarkable newBoard = place.board as IMarkable;
        Placement subsequent = null;

        // ��ȿ�� �˻� --------------------------------------
        if (!IsPlaceable(place, piece))
        {
            Debug.Log("������ �� ���� ��");
            return;
        }

        GameManager.Instance.TurnActionDecided = false;

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
        OnFinishAction?.Invoke();

        // �̺�Ʈ
        DialogueManager.Instance.CheckDialogueEvent(EndTurn);

        // �̺�Ʈ ���� Ȯ�� �� �� ����

        // �޸��� ���
        if(oldBoard != null && oldBoard == newBoard)
        {
            SaveMemento(piece, oldPlace, place, attackedPiece, subsequent);
        }
    }*/

    public void SaveMemento(Piece piece, Place prevPlace, Place nextPlace, Piece attackedPiece, Placement subsequent)
    {
        Placement newPlacement = new Placement(piece, prevPlace, nextPlace, attackedPiece, subsequent);
        // �޸��並 ���⼭ �����ؾ� �ұ�?
        placementRememberer.SaveMemento(newPlacement);
        Debug.Log("�޸��並 �����ߴ�");
    }

    public Piece MovePiece(Piece piece, Place place)
    {
        if(piece == null || place == null)
        {
            Debug.LogError(string.Format("null ��ü�� ���� {0}, {1}", piece, place));
            return null;
        }
         
        Place oldPlace = piece.place;


        influenceCalculator.InitInfluence(piece);

        Piece attackedPiece = piece.SetInPlace(place);    // �⹰�� ��� ��ġ �����

        if (attackedPiece != null)
        {
            Attack(piece, attackedPiece);
        }

        influenceCalculator.CalculateInfluence(piece);
        influenceCalculator.ApplyInfluence(piece);

        // �⹰�� �Ű��� ��, ��ȭ�� �ڸ��� �˸� �߼� (�⹰�� ���� ������ ������ �� �׻� ����)

        // ��ȭ�� ��ü�� ���
        AddChangedSubject(oldPlace);
        AddChangedSubject(place);

        //NotifyToObservers(oldPlace);
        //NotifyToObservers(place);

        return attackedPiece;
    }

    private void AddChangedSubject(ISubject subject)
    {
        if (null == subject)
            return;

        //�ߺ� �˻�
        for (int i = 0; i < changedSubjects.Count; i++)
        {
            if(changedSubjects[i] != subject)
            {
                continue;
            }
        }

        changedSubjects.Add(subject);
    }

    public void NotifyObservers()
    {
        for(int i = 0; i < changedSubjects.Count; i++)
        {
            changedSubjects[i].notifyObserver();
        }

        changedSubjects.Clear();
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnCoroutine());
    }
    private IEnumerator EndTurnCoroutine()
    {
        Piece endedPiece = selectedPiece;

        OnFinishAction?.Invoke();
        // �⺻ ��� �ð�
        yield return new WaitForSeconds(1.5f);

        Debug.Log("���� ���� ���� �Ǿ�����");

        SelectedPieceInit();

        /*IMarkable markable = endedPiece.place.board as IMarkable;
        if (markable != null)
        {
            markable.PostShowEnd(endedPiece);
            Debug.Log("ǥ�� ����");
        }
*/
        GameManager.Instance.SetNextState(GameManager.GameState.TURN_FINISHED);

        // ī�޶� ����
        OnLeaveSelect?.Invoke();
    }

    public void Attack(Piece piece, Piece target)
    {
        ExpelPiece(target);

        CameraController.Instance.AddToTargetGroup(piece.transform);
        //OnAttack?.Invoke();
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
