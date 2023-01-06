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

    public enum PlaceType { DEFENCE, ATTACK, MOVABLE, NORMAL, SPECIALMOVE}

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
    private Color highlight;
    [SerializeField]
    private Color attackable;


    public void ShowHUD()
    {
        Transform hud = SelectedPiece.place.board.heatPointHUD;
        if(hud != null) hud.gameObject.SetActive(true);
    }
    public void ShowMovable(Piece piece)
    {
        Place curPlace = piece.place;
        Vector2Int curIndex = curPlace.boardIndex;
        Board curBoard = curPlace.board;
       
        if (null == curBoard)                   // 기물이 있는 곳이 보드가 아니라면 종료
            return;

        if (!curBoard.FollowRule)               // 규칙을 따르지 않는 보드라면 종료
            return;

        // 연출
        List<Place> movable = selectedPiece.MovableTo;
        for(int i = 0; i < movable.Count; i++)
        {
            ChangePlaceColor(movable[i].boardIndex, PlaceType.MOVABLE);
        }
    }

    public void ShowInfluence(Piece piece)
    {
        Place curPlace = selectedPiece.place;
        Vector2Int curIndex = curPlace.boardIndex;
        Board curBoard = curPlace.board;

        if (null == curBoard)                   // 기물이 있는 곳이 보드가 아니라면 종료
            return;

        if (!curBoard.FollowRule)               // 규칙을 따르지 않는 보드라면 종료
            return;

        // 연출
        List<Place> influencing = piece.Influenceable;
        for (int i = 0; i < influencing.Count; i++)
        {
            //TODO: 영향권을 나타내는 색 따로 설정
            ChangePlaceColor(influencing[i].boardIndex, PlaceType.MOVABLE);
        }
    }

    public void ShowThreatAndDefence(Piece piece)
    {
        Place curPlace = piece.place;
        Vector2Int curIndex = curPlace.boardIndex;
        Board curBoard = curPlace.board;

        if (null == curBoard)                   // 기물이 있는 곳이 보드가 아니라면 종료
            return;

        if (!curBoard.FollowRule)               // 규칙을 따르지 않는 보드라면 종료
            return;

        // 연출
        List<Piece> defencing = selectedPiece.DefendFor;
        List<Piece> threating = selectedPiece.ThreatTo;

        for (int i = 0; i < defencing.Count; i++)
        {
            // 다른 보드로 위치가 변경될 시 문제 생길 수 있음
            ChangePlaceColor(defencing[i].place.boardIndex, PlaceType.DEFENCE);
            //defencing[i].place.ChangeColor();
        }
        for (int i = 0; i < threating.Count; i++)
        {
            ChangePlaceColor(threating[i].place.boardIndex, PlaceType.ATTACK);
            //threating[i].place.ChangeColor();
        }

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

    private void ShowMovableEnd(Piece endedPiece)
    {
        List<Place> movableList = endedPiece.MovableTo;
        for (int i = 0; i < movableList.Count; i++)
        {
            movableList[i].ChangeColor();
        }
    }
    private void ShowInfluenceEnd(Piece endedPiece)
    {
        List<Place> influenceList = endedPiece.Influenceable;
        for (int i = 0; i < influenceList.Count; i++)
        {
            influenceList[i].ChangeColor();
        }
    }
    private void ShowThreatAndDefenceEnd(Piece endedPiece)
    {
        List<Piece> defeceList = endedPiece.DefendFor;
        List<Piece> threatList = endedPiece.ThreatTo;

        for (int i = 0; i < defeceList.Count; i++)
        {
            defeceList[i].place.ChangeColor();
        }

        for (int i = 0; i < threatList.Count; i++)
        {
            threatList[i].place.ChangeColor();
        }
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

    public void ChangePlaceColor(Vector2Int location, PlaceType placeType)
    {
        switch(placeType)
        {
            case PlaceType.DEFENCE:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(Color.blue);
                break;

            case PlaceType.ATTACK:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(Color.red);
                break;

            case PlaceType.NORMAL:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor();
                break;

            case PlaceType.MOVABLE:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(highlight);
                break;

            case PlaceType.SPECIALMOVE:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(Color.gray);
                break;
        }
    }

    public void MovePieceTo(Place place)
    {
        Place oldPlace = selectedPiece.place;
        oldPlace.piece = null;

        // 이전 표시된 영역 지우기 연출
        PreShowEnd(selectedPiece);

        // 이전 영향력 제거
        WithDrawInfluence(SelectedPiece);
        selectedPiece.ClearMovable();

        // 영향권 비교 후 변동 사항에 대해 연산하기
        // =====임시 방편======

        selectedPiece.ClearThreat();
        selectedPiece.ClearDefence();
        selectedPiece.ClearInfluence();

        // ===================



        // Debug.Log(selectedPiece.MovableTo.Count.ToString());

        
        selectedPiece.SetInPlace(place);    // 기물이 밟는 위치 변경됨




        // 위치 변경 후 영향권 연산
        PostPlaceAction();

        place.board?.UpdateHeatHUD();

        // 영향권 연출
        PostShow(selectedPiece);

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

        //yield return new WaitForSeconds(1f);
        //PostShowEnd(endedPiece);
        ShowMovableEnd(endedPiece);
        ShowInfluenceEnd(endedPiece);

        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;

        // 카메라 연출
        OnNonSelectPiece?.Invoke();
    }


    private void PostShow(Piece finishedPiece)
    {
        ShowInfluence(finishedPiece);
        ShowThreatAndDefence(finishedPiece);
    }

    private void PreShow(Piece seleceted)
    {
        ShowMovable(seleceted);
        //ShowInfluence(seleceted);
        ShowThreatAndDefence(seleceted);
    }
    private void PreShowEnd(Piece endedPiece)
    {
        //ShowInfluence(endedPiece);
        ShowMovableEnd(endedPiece);
        ShowThreatAndDefenceEnd(endedPiece);
    }

    private IEnumerator PostShowEnd(Piece endedPiece)
    {
        yield return new WaitForSeconds(1f);
        //yield return null;

        ShowMovableEnd(endedPiece);
        //ShowThreatAndDefenceEnd(endedPiece);
        ShowInfluenceEnd(endedPiece);
    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PLACE;

        // 연출
        PreShow(piece);

        OnSelectPiece?.Invoke();

    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;

        // 선택된 기물을 바로 취소하는 경우

        //연출
        PreShowEnd(selectedPiece);

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
