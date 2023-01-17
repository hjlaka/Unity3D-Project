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

        piece.RecognizeRange(piece.place.boardIndex);

        // 계산 완료된 영향권의 과열도 추가
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
        //TODO: 최적화
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


        // 움직일 수 있는 곳인지 확인
        if (!IsPlaceable(place, piece))
        {
            Debug.Log("움직일 수 없는 곳");
            return;
        }
            

        
        // 연출 - 리스트 의존적.
        if (oldBoard != null)
            oldBoard.PreShowEnd(piece);


        MovePiece(piece, place);    
/*        // 연산
        oldPlace.Piece = null;
        InitInfluence(piece);

        
        // 연산
        piece.SetInPlace(place);    // 기물이 밟는 위치 변경됨
        CalculateInfluence(piece);*/


        // 연출 - 리스트 의존적 - 리스트를 받아올 수 있으면 좋을 것이다.
        if (newBoard != null)
        {
            newBoard.PostShow(piece);
        }
        

        // 연출
        OnFinishMove?.Invoke();

        // 이벤트
        DialogueManager.Instance.StartDialogue();

        // 이벤트 종료 확인 후 턴 종료
        StartCoroutine(EndTurn(piece));


        // 메멘토 등록
        if(oldBoard != null && newBoard.FollowRule && oldBoard == newBoard)
        {
            Placement newPlacement = new Placement(piece, oldPlace, place);   // 메멘토를 여기서 생성해야 할까?
            SaveMemento(newPlacement);
            Debug.Log("메멘토를 저장했다");
        }
            

    }

    private void MovePiece(Piece piece, Place place)
    {
        piece.place.Piece = null;
        InitInfluence(piece);


        // 연산
        piece.SetInPlace(place);    // 기물이 밟는 위치 변경됨
        CalculateInfluence(piece);
    }

    private IEnumerator EndTurn(Piece endedPiece)
    {
        // 기본 대기 시간
        yield return new WaitForSeconds(1.5f);

        while (GameManager.Instance.state != GameManager.GameState.SELECTING_PLACE)
        {
            yield return null;

            // 차례가 끝났다는 신호가 들어오길 기다린다.
        }
        Debug.Log("턴을 끝낼 때가 되었군요");

        SelectedPieceInit();

        MarkableBoard markableBoard = endedPiece.place.board as MarkableBoard;
        if (markableBoard != null)
        {
            markableBoard.ShowMovableEnd(endedPiece);
            markableBoard.ShowInfluenceEnd(endedPiece);
        }

        //GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;
        GameManager.Instance.ChangeGameState(GameManager.GameState.TURN_FINISHED);

        // 카메라 연출
        OnNonSelectPiece?.Invoke();
    }


    
    private void InitInfluence(Piece piece)
    {
        WithDrawInfluence(piece);

        piece.Recognized.ClearAllRecognized();

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


        // 변화된 상황이 있을 수 있으므로 재 계산
        InitInfluence(piece);

        CalculateInfluence(piece);



        // 연출
        MarkableBoard markableBoard = piece.place.board as MarkableBoard;
        if (markableBoard)
            markableBoard.PreShow(piece);
        //TODO: 이동 가능 상태 변수와 연출을 하나로 묶어도 좋을듯

        OnSelectPiece?.Invoke();

    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;

        // 선택된 기물을 바로 취소하는 경우

        //연출
        MarkableBoard markableBoard = selectedPiece.place.board as MarkableBoard;
        if (markableBoard)
            markableBoard.PreShowEnd(selectedPiece);

        SelectedPieceInit();
    }

    public void SelectedPieceInit()
    {
        Debug.Log("기물 선택 해제");
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
            Debug.Log("기물 선택 단계가 아님");
            return;
        }
        

        // 기본적으로 두번 복기한다.

        for(int i = 0; i < 2; i++)
        {
            Placement placement = placementRememberer.Get() as Placement;
            Debug.Log("복구 대상: " + placement.Piece);

            if (placement == null) return;

            Piece returnPiece = placement.Piece;
            Place returnPosition = placement.PrevPosition;
            GameManager.Instance.ChangeGameState(GameManager.GameState.RETURN);
            // 연출 없이 움직임만 복구
            // 복기한 움직임은 메멘토에 저장하지 않는다.
            MovePiece(returnPiece, returnPosition);
        }

        GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PIECE);

    }
}
