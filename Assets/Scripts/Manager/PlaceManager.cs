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


    [SerializeField]
    public Color highlight;
    [SerializeField]
    private Color attackable;


    public void ShowHUD()
    {
        Transform hud = SelectedPiece.place.board.heatPointHUD;
        if(hud != null) hud.gameObject.SetActive(true);
    }
    

    public void PostPlaceAction()
    {
        Place newPlace = selectedPiece.place;
        Vector2Int newIndex = newPlace.boardIndex;
        Board curBoard = newPlace.board;

        // 새로운 자리 과열도 추가
        if (selectedPiece.team.direction == TeamData.Direction.DownToUp)
            newPlace.HeatPointBottomTeam++;
        else
            newPlace.HeatPointTopTeam++;

        // 기물이 있는 곳이 보드가 아니라면 종료
        if (null == curBoard)
            return;

        // 규칙을 따르지 않는 보드라면 종료
        if (!curBoard.FollowRule)
            return;

        selectedPiece.IsMovable(selectedPiece.place.boardIndex);

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



    public void MovePieceTo(Place place)
    {
        Place oldPlace = selectedPiece.place;
        oldPlace.piece = null;
        Board oldBoard = oldPlace.board;

        if (oldBoard != null)
        {
            oldBoard.PreShowEnd(SelectedPiece);
        }
        WithDrawInfluence(SelectedPiece);
        selectedPiece.ClearMovable();

        // 영향권 비교 후 변동 사항에 대해 연산하기
        // =====임시 방편======

        selectedPiece.ClearThreat();
        selectedPiece.ClearDefence();
        selectedPiece.ClearInfluence();

        // ===================

        selectedPiece.SetInPlace(place);    // 기물이 밟는 위치 변경됨

        Board newBoard = place.board;


        // 위치 변경 후 영향권 연산
        PostPlaceAction();

        if(newBoard != null)
        {
            // 영향권 연출
            newBoard.UpdateHeatHUD();
            newBoard.PostShow(selectedPiece);
        }
        

        // 카메라 연출
        OnFinishMove?.Invoke();

        // 만약 진행할 이벤트가 있으면 실행
        DialogueManager.Instance.StartDialogue();


        // 이전 기물 저장
        Piece endedPiece = selectedPiece;


        StartCoroutine(EndTurn(endedPiece));        // 턴을 끝내는 연산을 진행할지 말지, 계속해서 확인

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


    

    

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PLACE;

        // 연출
        if(piece.place.board != null)
            piece.place.board.PreShow(piece);

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
