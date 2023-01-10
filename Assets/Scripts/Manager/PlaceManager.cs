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

            if (value != null && SelectedPiece.place.board?.heatPointHUD != null)
            {
                SelectedPiece.place.board.heatPointHUD.gameObject.SetActive(true);

            }
                
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


    public void ShowHUD()
    {
        Transform hud = SelectedPiece.place.board.heatPointHUD;
        if(hud != null) hud.gameObject.SetActive(true);
    }
    

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

        piece.IsMovable(piece.place.boardIndex);

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

        else
        {
            //movable�̾�� ������ ����.
            return place.IsMovableToCurPiece;
        }

    }
    public void ExpelPiece(Piece piece)
    {
        InitInfluence(piece);

        Destroy(piece.gameObject);
    }

    public void MovePieceTo(Piece piece, Place place)
    {
        Place oldPlace = piece.place;
        Board oldBoard = oldPlace.board;
        Board newBoard = place.board;


        // ������ �� �ִ� ������ Ȯ��
        if (!IsPlaceable(place, piece))
            return;

        
        // ���� - ����Ʈ ������.
        if (oldBoard != null)
            oldBoard.PreShowEnd(piece);


        // ����
        oldPlace.piece = null;
        WithDrawInfluence(piece);
        piece.ClearMovable();
        piece.ClearThreat();
        piece.ClearDefence();
        piece.ClearInfluence();

        
        // ����
        piece.SetInPlace(place);    // �⹰�� ��� ��ġ �����
        CalculateInfluence(piece);


        // ���� - ����Ʈ ������
        if(newBoard != null)
        {
            newBoard.UpdateHeatHUD();
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

        while (GameManager.Instance.state != GameManager.GameState.TURN_FINISHED)
        {
            yield return null;

            // ���ʰ� �����ٴ� ��ȣ�� ������ ��ٸ���.
        }
        Debug.Log("���� ���� ���� �Ǿ�����");

        SelectedPieceInit();

        Board endedBoard = endedPiece.place.board;
        if(endedBoard != null)
        {
            endedBoard.ShowMovableEnd(endedPiece);
            endedBoard.ShowInfluenceEnd(endedPiece);
        }

        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;

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
    
    public void UpdateHUD(Board board)
    {
        if (board == null) return;
        board.UpdateHeatHUD();
    }

    public void Attack(Piece piece, Piece target)
    {
        Place attackPlace = target.place;
        ExpelPiece(target);

        OnAttack?.Invoke();

        MovePieceTo(piece, attackPlace);
    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        SelectedPiece.ChangeColor(selectingColor);
        GameManager.Instance.state = GameManager.GameState.SELECTING_PLACE;


        // ��ȭ�� ��Ȳ�� ���� �� �����Ƿ� �� ���
        InitInfluence(piece);

        CalculateInfluence(piece);
        UpdateHUD(piece.place.board);



        // ����
        if (piece.place.board != null)
            piece.place.board.PreShow(piece);
        //TODO: �̵� ���� ���� ������ ������ �ϳ��� ��� ������

        OnSelectPiece?.Invoke();

    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;

        // ���õ� �⹰�� �ٷ� ����ϴ� ���

        //����
        if (selectedPiece.place.board != null)
            selectedPiece.place.board.PreShowEnd(selectedPiece);

        SelectedPieceInit();
    }

    public void SelectedPieceInit()
    {
        SelectedPiece.ChangeColor();

        SelectedPiece = null;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;

        //StartCoroutine(EndPlaceCam());
        
    }

    private IEnumerator EndPlaceCam()
    {
        yield return new WaitForSeconds(1f);

        OnNonSelectPiece?.Invoke();
    }
}
