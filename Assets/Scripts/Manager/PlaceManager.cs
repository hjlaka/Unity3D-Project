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
    private CameraController camController;

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

    public UnityEvent OnExitSelect;

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
        Vector2Int newIndex = newPlace.boardIndex;
        Board curBoard = newPlace.board;

        // ���ο� �ڸ� ������ �߰�

        if (piece.team.direction == TeamData.Direction.DownToUp)
            newPlace.HeatPointBottomTeam++;
        else
            newPlace.HeatPointTopTeam++;

        // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
        if (null == curBoard)
            return;

        // ��Ģ�� ������ �ʴ� ������ ����
        if (!curBoard.FollowRule)
            return;

        piece.RecognizeRange(piece.place.boardIndex);

        // ��� �Ϸ�� ������� ������ �߰�
        for(int i = 0; i < piece.Recognized.influenceable.Count; i++)
        {
            Place curPlace = piece.Recognized.influenceable[i];
            if (piece.team.direction == TeamData.Direction.DownToUp)
                curPlace.HeatPointBottomTeam++;
            else
                curPlace.HeatPointTopTeam++;

            curPlace.registerObserver(piece);
        }

    }

    public void ReCalculateInfluence(Piece piece)
    {
        InitInfluence(piece);
        CalculateInfluence(piece);
    }

    public void WithDrawInfluence(Piece leftPiece)
    {
        //TODO: ����ȭ
        if (leftPiece.team.direction == TeamData.Direction.DownToUp)
        {
            leftPiece.place.HeatPointBottomTeam--;
        }
        else
        {
            leftPiece.place.HeatPointTopTeam--;
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
            influencable[i].removeObserver(leftPiece);
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
        MarkableBoard oldBoard = oldPlace.board as MarkableBoard;
        MarkableBoard newBoard = place.board as MarkableBoard;


        // ������ �� �ִ� ������ Ȯ��
        if (!IsPlaceable(place, piece))
        {
            Debug.Log("������ �� ���� ��");
            return;
        }
            

        
        // ���� - ����Ʈ ������.
        if (oldBoard != null)
            oldBoard.PreShowEnd(piece);


        Piece attackedPiece = MovePiece(piece, place);    
/*        // ����
        oldPlace.Piece = null;
        InitInfluence(piece);

        
        // ����
        piece.SetInPlace(place);    // �⹰�� ��� ��ġ �����
        CalculateInfluence(piece);*/


        // ���� - ����Ʈ ������ - ����Ʈ�� �޾ƿ� �� ������ ���� ���̴�.
        if (newBoard != null)
        {
            newBoard.PostShow(piece);
        }
        

        // ����
        OnFinishMove?.Invoke();

        // �̺�Ʈ
        DialogueManager.Instance.StartDialogue();

        // �̺�Ʈ ���� Ȯ�� �� �� ����
        StartCoroutine(EndTurn(piece));


        // �޸��� ���
        if(oldBoard != null && newBoard.FollowRule && oldBoard == newBoard)
        {
            Placement newPlacement = new Placement(piece, oldPlace, place, attackedPiece);
            // �޸��並 ���⼭ �����ؾ� �ұ�? ���� ����� ���������� ��� Ȯ���ϴ°�?
            SaveMemento(newPlacement);
            Debug.Log("�޸��並 �����ߴ�");
        }
            

    }

    private Piece MovePiece(Piece piece, Place place)
    {
        Place oldPlace = piece.place;
        if(oldPlace != null)
            oldPlace.Piece = null;
        InitInfluence(piece);


        // ����
        Piece attackedPiece = piece.SetInPlace(place);    // �⹰�� ��� ��ġ �����

        if (attackedPiece != null)
        {
            Attack(piece, attackedPiece);
        }

        CalculateInfluence(piece);

        return attackedPiece;
    }

    private IEnumerator EndTurn(Piece endedPiece)
    {
        // �⺻ ��� �ð�
        yield return new WaitForSeconds(1.5f);

        while (GameManager.Instance.state != GameManager.GameState.SELECTING_PLACE)
        {
            yield return null;

            // ���ʰ� �����ٴ� ��ȣ�� ������ ��ٸ���.
        }
        Debug.Log("���� ���� ���� �Ǿ�����");

        SelectedPieceInit();

        MarkableBoard markableBoard = endedPiece.place.board as MarkableBoard;
        if (markableBoard != null)
        {
            markableBoard.ShowMovableEnd(endedPiece);
            markableBoard.ShowInfluenceEnd(endedPiece);
        }

        //GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;
        GameManager.Instance.ChangeGameState(GameManager.GameState.TURN_FINISHED);

        // ī�޶� ����
        OnNonSelectPiece?.Invoke();
    }


    
    private void InitInfluence(Piece piece)
    {
        WithDrawInfluence(piece);

        piece.Recognized.ClearAllRecognized();

    }

    public void Attack(Piece piece, Piece target)
    {
        ExpelPiece(target);
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
        GameManager.Instance.state = GameManager.GameState.SELECTING_PLACE;



        // ����
        MarkableBoard markableBoard = piece.place.board as MarkableBoard;
        if (markableBoard)
            markableBoard.PreShow(piece);
        //TODO: �̵� ���� ���� ������ ������ �ϳ��� ��� ������

        OnSelectPiece?.Invoke();

    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;

        // ���õ� �⹰�� �ٷ� ����ϴ� ���

        //����
        MarkableBoard markableBoard = selectedPiece.place.board as MarkableBoard;
        if (markableBoard)
            markableBoard.PreShowEnd(selectedPiece);

        SelectedPieceInit();
    }

    public void SelectedPieceInit()
    {
        Debug.Log("�⹰ ���� ����");
        SelectedPiece.ChangeColor();

        SelectedPiece = null;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;

        //StartCoroutine(EndPlaceCam());
        
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
        

        // �⺻������ �ι� �����Ѵ�.

        for(int i = 0; i < 2; i++)
        {
            Placement placement = placementRememberer.Get() as Placement;
            Debug.Log("���� ���: " + placement.Piece);

            if (placement == null) return;

            Piece returnPiece = placement.Piece;
            Place returnPosition = placement.PrevPosition;
            GameManager.Instance.ChangeGameState(GameManager.GameState.RETURN);
            // ���� ���� �����Ӹ� ����
            // ������ �������� �޸��信 �������� �ʴ´�.
            MovePiece(returnPiece, returnPosition);


            Piece capturedPiece = placement.CapturedPiece;
            
            if(capturedPiece != null)
            {
                Place capturedPlace = placement.NextPosition;
                // �⹰ ����
                Debug.Log("�⹰: " + capturedPiece + " ��ġ : " + capturedPlace);
                //MovePiece(capturedPiece, capturedPlace);
                capturedPiece.SetInPlace(capturedPlace);
                CalculateInfluence(capturedPiece);
                capturedPiece.IsFree = false;
            }
        }

        GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PIECE);

    }
}
