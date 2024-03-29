using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceManager : SingleTon<PlaceManager>
{
    [SerializeField]
    private Piece selectedPiece;
    public Piece SelectedPiece { get { return selectedPiece; } set { selectedPiece = value; /* 값 검증 추가?*/ } }

    // ========== Event ==============
    public UnityEvent<Piece> OnSelectPiece;
    public UnityEvent OnLeaveSelect;
    public UnityEvent OnFinishAction;
    public UnityEvent<Piece> OnStartAction;
    public UnityEvent OnAttack;
    //================================

    //======== Color Setting =========
    [Header("Piece")]
    [SerializeField]
    private Color selectingColor;
    [Header("Place")]
    [SerializeField]
    public Color highlight;
    [SerializeField]
    private Color attackable;
    //================================

    //============ Facade =============
    private InfluenceCalculator influenceCalculator;
    private PlacementRememberer placementRememberer;
    //=================================

    //========== Turn Event ===========
    private TurnEvent turnEventPeace;
    private TurnEvent turnEventAttack;
    //=================================

    //========== Observer =============
    private List<ISubject> changedSubjects { get; set; }
    //=================================

    private void Awake()
    {
        influenceCalculator = GetComponentInChildren<InfluenceCalculator>();
        placementRememberer = GetComponentInChildren<PlacementRememberer>();

        turnEventPeace = GetComponentInChildren<TurnEventPeace>();
        turnEventAttack = GetComponentInChildren<TurnEventAttack>();

        changedSubjects = new List<ISubject>();
    }

    public void SelectPiece(Piece piece)
    {
        // 수정 예정
        if (!GameManager.Instance.TurnActionDecided)
        {
            Debug.Log("선택 허용 안됨");
            return; 
        }

        SelectedPiece = piece;
        SelectedPiece.ChangeColor(selectingColor);
/*        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
            GameManager.Instance.ChangeGameState(GameManager.GameState.SELECTING_PLACE);*/

        // 움직일 수 있는 범위 등록
        piece.place?.board?.ApplyMovable(piece, true);

        // 연출
        OnSelectPiece?.Invoke(SelectedPiece);
        OnLeaveSelect.AddListener(SelectedPiece.ChangeColor);
        //TODO: 이동 가능 상태 변수와 연출을 하나로 묶어도 좋을듯
    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;
        // 선택된 기물을 이동하지 않고 바로 취소하는 경우

        // 움직일 수 있는 범위 등록 해제
        selectedPiece.place?.board?.ApplyMovable(selectedPiece, false);

        //연출
        IMarkable markable = selectedPiece.place.board as IMarkable;
        if (markable != null)
            markable.PreShowEnd(selectedPiece);

        SelectedPieceInit();
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

    public void GetWill(Piece subject, IOnBoardTargetable target)
    {
        Debug.Log("의지 수신");
        // 유효성 검사

        if (!GameManager.Instance.TurnActionDecided)
        {
            Debug.Log("유효하지 않은 선택");
            return;
        }
        if (!CheckTargetValidity(target))
        {
            Debug.Log("이동 불가한 위치");
            return;
        }
        GameManager.Instance.TurnActionDecided = false;
        OnStartAction?.Invoke(selectedPiece);

        // 턴 이벤트 생성

        IOnBoardTargetable.Type turnType = target.React();

        

        switch(turnType)
        {
            case IOnBoardTargetable.Type.Peace:
                turnEventPeace.SetTurnEvent(subject, target);
                turnEventPeace.DoTurn();
                break;
            case IOnBoardTargetable.Type.Attack:
                turnEventAttack.SetTurnEvent(subject, target);
                turnEventAttack.DoTurn();
                break;
        }
    }

    private bool CheckTargetValidity(IOnBoardTargetable targetable)
    {
        Debug.Log(string.Format("{0}가 {1}에게 의지 발산", selectedPiece, targetable));
        if (targetable is Place)
        {

            return (targetable as Place).IsMovableToCurPiece;
 
        }
        else if (targetable is Piece)
        {
            return (targetable as Piece).place.IsMovableToCurPiece;

        }
        Debug.Log("이동 불가한 위치");
        return false;
    }



    private bool IsPlaceable(Place place, Piece piece)
    {
        if (place.board == null)
        {
            Debug.Log("보드가 아닌 위치");
            return true;
        }

        if (place.board != piece.place.board)
        {
            Debug.Log("보드가 이전 보드와 다른 위치");
            return true;
        }
        
        if (!place.board.FollowRule)
        {
            Debug.Log("규칙을 따르지 않는 보드");
            return true;
        }
        

        return place.IsMovableToCurPiece;
    }


    // 어댑터? 중간자 역할? 필요할까? (내부에서는 이 어댑터를 써야 할까?)
    public void CalculateInfluence(Piece piece)
    {
        influenceCalculator.CalculateInfluence(piece);
    }
    public void ApplyInfluence(Piece piece)
    {
        influenceCalculator.ApplyInfluence(piece);
    }
    public void ReCalculateInfluence(Piece piece)
    {
        influenceCalculator.ReCalculateInfluence(piece);
    }


    /*public void MoveProcess(Piece piece, Place place)
    {
        Place oldPlace = piece.place;
        IMarkable oldBoard = oldPlace?.board as IMarkable;
        IMarkable newBoard = place.board as IMarkable;
        Placement subsequent = null;

        // 유효성 검사 --------------------------------------
        if (!IsPlaceable(place, piece))
        {
            Debug.Log("움직일 수 없는 곳");
            return;
        }

        GameManager.Instance.TurnActionDecided = false;

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
        OnFinishAction?.Invoke();

        // 이벤트
        DialogueManager.Instance.CheckDialogueEvent(EndTurn);

        // 이벤트 종료 확인 후 턴 종료

        // 메멘토 등록
        if(oldBoard != null && oldBoard == newBoard)
        {
            SaveMemento(piece, oldPlace, place, attackedPiece, subsequent);
        }
    }*/

    public void SaveMemento(Piece piece, Place prevPlace, Place nextPlace, Piece attackedPiece, Placement subsequent)
    {
        Placement newPlacement = new Placement(piece, prevPlace, nextPlace, attackedPiece, subsequent);
        // 메멘토를 여기서 생성해야 할까?
        placementRememberer.SaveMemento(newPlacement);
        Debug.Log("메멘토를 저장했다");
    }

    public Piece MovePiece(Piece piece, Place place)
    {
        if(piece == null || place == null)
        {
            Debug.LogError(string.Format("null 객체가 들어옴 {0}, {1}", piece, place));
            return null;
        }
         
        Place oldPlace = piece.place;


        influenceCalculator.InitInfluence(piece);

        Piece attackedPiece = piece.SetInPlace(place);    // 기물이 밟는 위치 변경됨

        if (attackedPiece != null)
        {
            Attack(piece, attackedPiece);
        }

        influenceCalculator.CalculateInfluence(piece);
        influenceCalculator.ApplyInfluence(piece);

        // 기물을 옮겼을 때, 변화된 자리들 알림 발송 (기물을 보드 위에서 움직일 때 항상 적용)

        // 변화된 주체로 등록
        AddChangedSubject(oldPlace);
        AddChangedSubject(place);

        //NotifyToObservers(oldPlace);
        //NotifyToObservers(place);

        return attackedPiece;
    }

    private void AddChangedSubject(ISubject subject)
    {
        if (null == subject)
            return;

        //중복 검사
        for (int i = 0; i < changedSubjects.Count; i++)
        {
            if(changedSubjects[i] != subject)
            {
                continue;
            }
        }

        changedSubjects.Add(subject);
    }

    public void NotifyObservers()
    {
        for(int i = 0; i < changedSubjects.Count; i++)
        {
            changedSubjects[i].NotifyObserver();
        }

        changedSubjects.Clear();
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnCoroutine());
    }
    private IEnumerator EndTurnCoroutine()
    {
        OnFinishAction?.Invoke();
        // 기본 대기 시간
        yield return new WaitForSeconds(1.5f);

        Debug.Log("턴을 끝낼 때가 되었군요");

        SelectedPieceInit();

        /*IMarkable markable = endedPiece.place.board as IMarkable;
        if (markable != null)
        {
            markable.PostShowEnd(endedPiece);
            Debug.Log("표시 종료");
        }
*/
        GameManager.Instance.SetNextState(GameManager.GameState.TURN_FINISHED);

        // 카메라 연출
        OnLeaveSelect?.Invoke();
    }

    public void Attack(Piece piece, Piece target)
    {
        ExpelPiece(target);

        CameraController.Instance.AddToTargetGroup(piece.transform);
        //OnAttack?.Invoke();
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
