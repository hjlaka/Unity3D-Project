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

        // 새로운 자리 과열도 추가
        if (piece.team.direction == TeamData.Direction.DownToUp)
            newPlace.HeatPointBottomTeam++;
        else
            newPlace.HeatPointTopTeam++;

        // 기물이 있는 곳이 보드가 아니라면 종료
        if (null == curBoard)
            return;

        // 규칙을 따르지 않는 보드라면 종료
        if (!curBoard.FollowRule)
            return;

        piece.IsMovable(piece.place.boardIndex);

    }

    

    public void WithDrawInfluence(Piece leftPiece)
    {
        //TODO: 최적화
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
            //movable이어야 움직임 가능.
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


        // 움직일 수 있는 곳인지 확인
        if (!IsPlaceable(place, piece))
            return;

        
        // 연출 - 리스트 의존적.
        if (oldBoard != null)
            oldBoard.PreShowEnd(piece);


        // 연산
        oldPlace.piece = null;
        WithDrawInfluence(piece);
        piece.ClearMovable();
        piece.ClearThreat();
        piece.ClearDefence();
        piece.ClearInfluence();

        
        // 연산
        piece.SetInPlace(place);    // 기물이 밟는 위치 변경됨
        CalculateInfluence(piece);


        // 연출 - 리스트 의존적
        if(newBoard != null)
        {
            newBoard.UpdateHeatHUD();
            newBoard.PostShow(piece);
        }
        

        // 연출
        OnFinishMove?.Invoke();

        // 이벤트
        DialogueManager.Instance.StartDialogue();

        // 이벤트 종료 확인 후 턴 종료
        StartCoroutine(EndTurn(piece));        

    }

    private IEnumerator EndTurn(Piece endedPiece)
    {
        // 기본 대기 시간
        yield return new WaitForSeconds(1.5f);

        while (GameManager.Instance.state != GameManager.GameState.TURN_FINISHED)
        {
            yield return null;

            // 차례가 끝났다는 신호가 들어오길 기다린다.
        }
        Debug.Log("턴을 끝낼 때가 되었군요");

        SelectedPieceInit();

        Board endedBoard = endedPiece.place.board;
        if(endedBoard != null)
        {
            endedBoard.ShowMovableEnd(endedPiece);
            endedBoard.ShowInfluenceEnd(endedPiece);
        }

        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;

        // 카메라 연출
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


        // 변화된 상황이 있을 수 있으므로 재 계산
        InitInfluence(piece);

        CalculateInfluence(piece);
        UpdateHUD(piece.place.board);



        // 연출
        if (piece.place.board != null)
            piece.place.board.PreShow(piece);
        //TODO: 이동 가능 상태 변수와 연출을 하나로 묶어도 좋을듯

        OnSelectPiece?.Invoke();

    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;

        // 선택된 기물을 바로 취소하는 경우

        //연출
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
