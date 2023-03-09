using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceManager : SingleTon<PlaceManager>, IOriginator
{
    [SerializeField]
    private Piece selectedPiece;

    [SerializeField]
    private PlacementRememberer placementRememberer;

    public UnityEvent OnSelectPiece;
    public UnityEvent OnNonSelectPiece;
    public UnityEvent OnFinishMove;
    public UnityEvent OnAttack;



    private Coroutine waitToInit;
    private Coroutine showEnd;
    public Piece SelectedPiece
    {
        get { return selectedPiece; }
        set 
        { 
            selectedPiece = value;           
        }
    }

    public UnityEvent OnLeaveSelect;
    public UnityEvent OnEnterSelect;
    public UnityEvent OnEndMove;
    public UnityEvent OnStartMove;

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

    

    
    public void CalculateInfluence(Piece piece)
    {
        Place newPlace = piece.place;
        Board curBoard = newPlace.board;

        // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
        if (null == curBoard)
            return;

        // ��Ģ�� ������ �ʴ� ������ ����
        if (!curBoard.FollowRule)
            return;

        piece.RecognizeRange(piece.place.boardIndex);

    }

    public void ApplyInfluence(Piece piece)
    {
        Place curPlace = piece.place;
        // ���ο� �ڸ� ������ �߰�

        Debug.Log("���� �ڸ� ������ �߰�");
        if (piece.team.direction == TeamData.Direction.DownToUp)
            curPlace.HeatPointBottomTeam++;
        else
            curPlace.HeatPointTopTeam++;

        // ��� �Ϸ�� ������� ������ �߰�
        for (int i = 0; i < piece.Recognized.influenceable.Count; i++)
        {
            Place iterPlace = piece.Recognized.influenceable[i];
            if (piece.team.direction == TeamData.Direction.DownToUp)
                iterPlace.HeatPointBottomTeam++;
            else
                iterPlace.HeatPointTopTeam++;

            iterPlace.registerObserver(piece.PlaceObserver);
        }

        for(int i = 0; i < piece.Recognized.special.Count; i++)
        {
            Place iterPlace = piece.Recognized.special[i];

            iterPlace.registerObserver(piece.PlaceObserver);
        }
    }

    public void ReCalculateInfluence(Piece piece)
    {
        Debug.Log(piece + "���� ����");
        InitInfluence(piece);
        CalculateInfluence(piece);
        ApplyInfluence(piece);
    }


    public void WithDrawInfluence(Piece leftPiece)
    {
        Place leftPlace = leftPiece.place;

        if (leftPlace == null) return;

        if (leftPiece.team.direction == TeamData.Direction.DownToUp)
        {
            leftPlace.HeatPointBottomTeam--;
        }
        else
        {
            leftPlace.HeatPointTopTeam--;
        }

        List<Place> influencable = leftPiece.Recognized.influenceable;
        for (int i = 0; i < influencable.Count; i++)
        {
            if (leftPiece.team.direction == TeamData.Direction.DownToUp)
            {
                influencable[i].HeatPointBottomTeam--;
            }
            else
            {
                influencable[i].HeatPointTopTeam--;
            }
            influencable[i].removeObserver(leftPiece.PlaceObserver);
        }

        for (int i = 0; i < leftPiece.Recognized.special.Count; i++)
        {
            Place curPlace = leftPiece.Recognized.special[i];

            curPlace.removeObserver(leftPiece.PlaceObserver);
        }
    }


    private bool IsPlaceable(Place place, Piece piece)
    {
        if (place.board == null) return true;
        if (place.board != piece.place.board) return true;
        if (!place.board.FollowRule) return true;

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
            SaveMemento(newPlacement);
            Debug.Log("�޸��並 �����ߴ�");
        }
            

    }

    public Piece MovePiece(Piece piece, Place place)
    {
        if(piece == null || place == null) 
            return null;

        Place oldPlace = piece.place;
        if(oldPlace != null)
            InitInfluence(piece);

        // ����
        Piece attackedPiece = piece.SetInPlace(place);    // �⹰�� ��� ��ġ �����

        if (attackedPiece != null)
        {
            Attack(piece, attackedPiece);
        }

        CalculateInfluence(piece);
        ApplyInfluence(piece);

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
        OnNonSelectPiece?.Invoke();
    }


    
    public void InitInfluence(Piece piece)
    {
        WithDrawInfluence(piece);

        piece.Recognized.ClearAllRecognized();

    }

    public void Attack(Piece piece, Piece target)
    {
        ExpelPiece(target);

        CameraController.Instance.AddToTargetGroup(piece.transform);
        OnAttack?.Invoke();
        // ������ �̺�Ʈ ���?
        // �ǰ����� �̺�Ʈ ���?
    }

    public void ExpelPiece(Piece piece)
    {
        InitInfluence(piece);
        piece.place = null;
        piece.transform.Translate(new Vector3(0, 0, 0));
        //MovePiece(piece, expelZone);
        piece.IsFree = true;
    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        SelectedPiece.ChangeColor(selectingColor);
        if(GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
            GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PLACE);


        // ����
        IMarkable markable = piece.place.board as IMarkable;
        if (markable != null)
            markable.PreShow(piece);
        //TODO: �̵� ���� ���� ������ ������ �ϳ��� ��� ������

        OnSelectPiece?.Invoke();

    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;

        // ���õ� �⹰�� �ٷ� ����ϴ� ���

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
        SelectedPiece.ChangeColor();

        SelectedPiece = null;
        
    }

    public IMemento SaveMemento(IMemento memento)
    {
        placementRememberer.Add(memento);
        return memento;
    }

    public void ApplyMemento()
    {
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE)
        {
            Debug.Log("�⹰ ���� �ܰ谡 �ƴ�");
            return;
        }

        GameManager.Instance.ChangeTurn(GameManager.TurnState.RETURN);

        // �⺻������ �ι� �����Ѵ�.

        for (int i = 0; i < 2; i++)
        {
            Placement placement = placementRememberer.Get() as Placement;
            if (placement == null) return;
            Debug.Log("���� ���: " + placement.Piece);

            // ������ �� ���� ����
            // TODO: ������ ������ ���ٸ� �Ŀ� ��͹����� ���� / Ȥ�� �迭 ó���� ����
            if (placement.Subsequent != null)
            {
                Placement subsequent = placement.Subsequent;
                Piece subsequentPiece = subsequent.Piece;
                Place subsequentPosition = subsequent.PrevPosition;

                MovePiece(subsequentPiece, subsequentPosition);

                Piece subsequentCaptured = subsequent.CapturedPiece;
                if (subsequentCaptured != null)
                {
                    MovePiece(subsequentCaptured, subsequent.NextPosition);
                    subsequentCaptured.IsFree = false;
                }
            }

            Piece returnPiece = placement.Piece;
            Place returnPosition = placement.PrevPosition;
          
            // ���� ���� �����Ӹ� ����
            // ������ �������� �޸��信 �������� �ʴ´�.
            MovePiece(returnPiece, returnPosition);

            Piece capturedPiece = placement.CapturedPiece;
            
            if(capturedPiece != null)
            {
                Place capturedPlace = placement.NextPosition;
                // �⹰ ����
                Debug.Log("�⹰: " + capturedPiece + " ��ġ : " + capturedPlace);
                // MovePiece �Լ��� ���� ���ؼ��� ����ó���� �� �ʿ��ϴ�.
                MovePiece(capturedPiece, capturedPlace);
                //capturedPlace.notifyObserver();
                capturedPiece.IsFree = false;

                // �ӽ� �̺�Ʈ �߰� ó��
                //ChessEvent reviveEvent = new ChessEvent(ChessEvent.EventType.RETURN, capturedPiece, null);
                //ChessEventManager.Instance.AddEvent(reviveEvent);
            }

            // �ӽ� �̺�Ʈ �߰� ó��
            //ChessEvent returnEvent = new ChessEvent(ChessEvent.EventType.RETURN, returnPiece, null);
            //ChessEventManager.Instance.AddEvent(returnEvent);

        }
        GameManager.Instance.ChangeGameState(GameManager.GameState.TURN_FINISHED);
        // �̺�Ʈ Ȯ�� ���� ���� �Ŵ������� ó��

    }
}
