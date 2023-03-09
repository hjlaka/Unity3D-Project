using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceManager : SingleTon<PlaceManager>
{
    [SerializeField]
    private Piece selectedPiece;
    public Piece SelectedPiece { get { return selectedPiece; } set { selectedPiece = value; } }

    [SerializeField]
    private PlacementRememberer placeRememberer;

    // ========== Event ==============
    public UnityEvent<Piece> OnSelectPiece;
    public UnityEvent OnLeaveSelect;
    public UnityEvent OnFinishMove;
    public UnityEvent OnStartMove;
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
    private InfluenceCalculator influenceCalculator;
    //=================================

    private void Awake()
    {
        influenceCalculator = GetComponentInChildren<InfluenceCalculator>();
    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        SelectedPiece.ChangeColor(selectingColor);
        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
            GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PLACE);

        // 연출
        OnSelectPiece?.Invoke(SelectedPiece);
        OnLeaveSelect.AddListener(SelectedPiece.ChangeColor);
        //TODO: 이동 가능 상태 변수와 연출을 하나로 묶어도 좋을듯
    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;
        // 선택된 기물을 이동하지 않고 바로 취소하는 경우

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
        //선택된 기물이 

        if (selectedPiece == null)
            return;

        OnLeaveSelect?.Invoke();
        SelectedPiece.ChangeColor();

        SelectedPiece = null;
    }

    public void ShowOnBoard(Piece piece)
    {
        // 어떤 보드에서 연출이 되어야 할지 찾는 과정
        IMarkable markable = piece.place.board as IMarkable;
        if (markable != null)
            markable.PreShow(piece);
    }

    private bool IsPlaceable(Place place, Piece piece)
    {
        if (place.board == null) 
            return true;
        if (place.board != piece.place.board) 
            return true;
        if (!place.board.FollowRule) 
            return true;

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

        // 기물 이동
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
            placeRememberer.SaveMemento(newPlacement);
            Debug.Log("메멘토를 저장했다");
        }
    }

    public Piece MovePiece(Piece piece, Place place)
    {
        if(piece == null || place == null) 
            return null;

        Place oldPlace = piece.place;
        if(oldPlace != null)
            influenceCalculator.InitInfluence(piece);

        // 연산
        Piece attackedPiece = piece.SetInPlace(place);    // 기물이 밟는 위치 변경됨

        if (attackedPiece != null)
        {
            Attack(piece, attackedPiece);
        }

        influenceCalculator.CalculateInfluence(piece);
        influenceCalculator.ApplyInfluence(piece);

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
        OnLeaveSelect?.Invoke();
    }

    public void Attack(Piece piece, Piece target)
    {
        ExpelPiece(target);

        CameraController.Instance.AddToTargetGroup(piece.transform);
        OnAttack?.Invoke();
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
