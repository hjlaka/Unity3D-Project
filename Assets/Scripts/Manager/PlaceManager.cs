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

        // 기물이 있는 곳이 보드가 아니라면 종료
        if (null == curBoard)
            return;

        // 규칙을 따르지 않는 보드라면 종료
        if (!curBoard.FollowRule)
            return;

        piece.RecognizeRange(piece.place.boardIndex);

    }

    public void ApplyInfluence(Piece piece)
    {
        Place curPlace = piece.place;
        // 새로운 자리 과열도 추가

        Debug.Log("지금 자리 과열도 추가");
        if (piece.team.direction == TeamData.Direction.DownToUp)
            curPlace.HeatPointBottomTeam++;
        else
            curPlace.HeatPointTopTeam++;

        // 계산 완료된 영향권의 과열도 추가
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
        Debug.Log(piece + "영향 재계산");
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


        // 움직일 수 있는 곳인지 확인
        if (!IsPlaceable(place, piece))
        {
            Debug.Log("움직일 수 없는 곳");
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
            

        
        // 연출 - 리스트 의존적.
        if (oldBoard != null)
            oldBoard.PreShowEnd(piece);


        Piece attackedPiece = MovePiece(piece, place);

        
        if(oldPlace.MoveAction != null)
        {
            subsequent = oldPlace.MoveAction.DoAction();
        }


        // 연출 - 리스트 의존적 - 리스트를 받아올 수 있으면 좋을 것이다.
        if (newBoard != null)
        {
            newBoard.PostShow(piece);
        }
        

        // 연출
        OnFinishMove?.Invoke();

        // 이벤트
        DialogueManager.Instance.CheckDialogueEvent();

        // 이벤트 종료 확인 후 턴 종료
        StartCoroutine(EndTurn(piece, true));


        // 메멘토 등록
        if(oldBoard != null && oldBoard == newBoard)
        {
            Placement newPlacement = new Placement(piece, oldPlace, place, attackedPiece, subsequent);
            // 메멘토를 여기서 생성해야 할까?
            SaveMemento(newPlacement);
            Debug.Log("메멘토를 저장했다");
        }
            

    }

    public Piece MovePiece(Piece piece, Place place)
    {
        if(piece == null || place == null) 
            return null;

        Place oldPlace = piece.place;
        if(oldPlace != null)
            InitInfluence(piece);

        // 연산
        Piece attackedPiece = piece.SetInPlace(place);    // 기물이 밟는 위치 변경됨

        if (attackedPiece != null)
        {
            Attack(piece, attackedPiece);
        }

        CalculateInfluence(piece);
        ApplyInfluence(piece);

        // 기물을 옮겼을 때, 변화된 자리들 알림 발송
        oldPlace?.notifyObserver();
        place.notifyObserver();

        return attackedPiece;
    }

    private IEnumerator EndTurn(Piece endedPiece, bool endMark)
    {
        // 기본 대기 시간
        yield return new WaitForSeconds(1.5f);

        while (GameManager.Instance.state == GameManager.GameState.IN_CONVERSATION)
        {
            yield return null;

            // 차례가 끝났다는 신호가 들어오길 기다린다.
        }
        Debug.Log("턴을 끝낼 때가 되었군요");

        SelectedPieceInit();

        if(endMark)
        {
            IMarkable markable = endedPiece.place.board as IMarkable;
            if (markable != null)
            {
                markable.PostShowEnd(endedPiece);
                Debug.Log("표시 종료");
            }
        }

        GameManager.Instance.ChangeGameState(GameManager.GameState.TURN_FINISHED);

        // 카메라 연출
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
        // 공격자 이벤트 등록?
        // 피공격자 이벤트 등록?
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


        // 연출
        IMarkable markable = piece.place.board as IMarkable;
        if (markable != null)
            markable.PreShow(piece);
        //TODO: 이동 가능 상태 변수와 연출을 하나로 묶어도 좋을듯

        OnSelectPiece?.Invoke();

    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;

        // 선택된 기물을 바로 취소하는 경우

        //연출
        IMarkable markable = selectedPiece.place.board as IMarkable;
        if (markable != null)
            markable.PreShowEnd(selectedPiece);

        SelectedPieceInit();
        GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PIECE);
    }

    public void SelectedPieceInit()
    {
        Debug.Log("기물 선택 해제");
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
            Debug.Log("기물 선택 단계가 아님");
            return;
        }

        GameManager.Instance.ChangeTurn(GameManager.TurnState.RETURN);

        // 기본적으로 두번 복기한다.

        for (int i = 0; i < 2; i++)
        {
            Placement placement = placementRememberer.Get() as Placement;
            if (placement == null) return;
            Debug.Log("복구 대상: " + placement.Piece);

            // 연달은 수 먼저 복구
            // TODO: 오류의 여지가 없다면 후에 재귀문으로 변경 / 혹은 배열 처리로 변경
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
          
            // 연출 없이 움직임만 복구
            // 복기한 움직임은 메멘토에 저장하지 않는다.
            MovePiece(returnPiece, returnPosition);

            Piece capturedPiece = placement.CapturedPiece;
            
            if(capturedPiece != null)
            {
                Place capturedPlace = placement.NextPosition;
                // 기물 복구
                Debug.Log("기물: " + capturedPiece + " 위치 : " + capturedPlace);
                // MovePiece 함수를 쓰기 위해서는 예외처리가 더 필요하다.
                MovePiece(capturedPiece, capturedPlace);
                //capturedPlace.notifyObserver();
                capturedPiece.IsFree = false;

                // 임시 이벤트 추가 처리
                //ChessEvent reviveEvent = new ChessEvent(ChessEvent.EventType.RETURN, capturedPiece, null);
                //ChessEventManager.Instance.AddEvent(reviveEvent);
            }

            // 임시 이벤트 추가 처리
            //ChessEvent returnEvent = new ChessEvent(ChessEvent.EventType.RETURN, returnPiece, null);
            //ChessEventManager.Instance.AddEvent(returnEvent);

        }
        GameManager.Instance.ChangeGameState(GameManager.GameState.TURN_FINISHED);
        // 이벤트 확인 동작 게임 매니저에서 처리

    }
}
