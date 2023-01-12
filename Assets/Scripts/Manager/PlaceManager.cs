using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceManager : SingleTon<PlaceManager>
{
    [SerializeField]
    private Piece selectedPiece;

    [SerializeField]
    private CameraController camController;


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
        

        List<Place> influencable = leftPiece.Influenceable;

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
        }

    }


    private bool IsPlaceable(Place place, Piece piece)
    {
        if (place.board == null) return true;
        if (place.board != piece.place.board) return true;
        if (!place.board.FollowRule) return true;

        return place.IsMovableToCurPiece;

    }
    public void ExpelPiece(Piece piece)
    {
        InitInfluence(piece);

        Destroy(piece.gameObject);
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


        // ����
        oldPlace.piece = null;
        InitInfluence(piece);

        
        // ����
        piece.SetInPlace(place);    // �⹰�� ��� ��ġ �����
        CalculateInfluence(piece);


        // ���� - ����Ʈ ������
        if(newBoard != null)
        {
            newBoard.PostShow(piece);
        }
        

        // ����
        OnFinishMove?.Invoke();

        // �̺�Ʈ
        DialogueManager.Instance.StartDialogue();

        // �̺�Ʈ ���� Ȯ�� �� �� ����
        StartCoroutine(EndTurn(piece));        

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
        GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PIECE);

        // ī�޶� ����
        OnNonSelectPiece?.Invoke();
    }


    
    private void InitInfluence(Piece piece)
    {
        WithDrawInfluence(piece);

        piece.ClearMovable();
        piece.ClearThreat();
        piece.ClearDefence();
        piece.ClearInfluence();

    }
    


    public void Attack(Piece piece, Piece target)
    {
        Place attackPlace = target.place;
        ExpelPiece(target);

        OnAttack?.Invoke();

        MoveProcess(piece, attackPlace);
    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        SelectedPiece.ChangeColor(selectingColor);
        GameManager.Instance.state = GameManager.GameState.SELECTING_PLACE;


        // ��ȭ�� ��Ȳ�� ���� �� �����Ƿ� �� ���
        InitInfluence(piece);

        CalculateInfluence(piece);



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
        SelectedPiece.ChangeColor();

        SelectedPiece = null;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;

        //StartCoroutine(EndPlaceCam());
        
    }


}
