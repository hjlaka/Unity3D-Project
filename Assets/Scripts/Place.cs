using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Place : MonoBehaviour, ISubject, ITargetable
{
    public enum PlaceType { A, B, V };
    private Piece piece;

    public Board board;
    public Vector2Int boardIndex;
    public PlaceType type;     // 스크립터블 오브젝트
    private Color typeColor;


    [Header("Running Game")]

    [SerializeField]
    private bool isMovableToCurPiece = false;
    [SerializeField]
    private bool isAttackableByCurPiece = false;
    private SpecialMove moveAction;

    [SerializeField]
    private List<PlaceObserver> influencingUnit;

    [SerializeField]
    private PlaceEffect effect;

    public Piece Piece
    {
        get { return piece; }
        set { piece = value; }
    }
    public bool IsMovableToCurPiece
    {
        get { return isMovableToCurPiece; }
        set { isMovableToCurPiece = value; }
    }
    public bool IsAttackableByCurPiece
    {
        get { return isAttackableByCurPiece; }
        set { isAttackableByCurPiece = value; }
    }

    public SpecialMove MoveAction
    {
        get { return moveAction; }
        set { moveAction = value; }
    }

    [Header("UI Setting")]

    [SerializeField]
    private TextMeshProUGUI topTeamHeatUI;
    [SerializeField]
    private TextMeshProUGUI TotalHeatUI;
    [SerializeField]
    private TextMeshProUGUI bottomTeamHeatUI;
    [SerializeField]
    private int heatPointTopTeam;
    [SerializeField]
    private int heatPointBottomTeam;
    [SerializeField]
    private int heatPoint;

    #region 프로퍼티
    public int HeatPointTopTeam
    {
        get { return heatPointTopTeam; }
        set
        {
            heatPointTopTeam = value; HeatPoint = heatPointBottomTeam + heatPointTopTeam;
            //Debug.Log(string.Format("위팀 과열도 {0}, 전체 과열도 {1}", heatPointTopTeam, heatPoint));
            if (topTeamHeatUI != null) topTeamHeatUI.text = heatPointTopTeam.ToString();
        }
    }

    public int HeatPointBottomTeam
    {
        get { return heatPointBottomTeam; }
        set
        {
            heatPointBottomTeam = value; HeatPoint = heatPointBottomTeam + heatPointTopTeam;
            //Debug.Log(string.Format("아래팀 과열도 {0}, 전체 과열도 {1}", heatPointBottomTeam, heatPoint));
            if (bottomTeamHeatUI != null) bottomTeamHeatUI.text = heatPointBottomTeam.ToString();
        }
    }

    public int HeatPoint
    {
        get
        {
            return heatPoint;
        }
        set
        {
            heatPoint = value;

            if (heatPoint < 0) heatPoint = 0;
            if (effect != null)
                effect.Intencity = heatPoint * 0.2f;


            //if (heatPoint >= 3) Debug.Log(heatPoint + " 과열된 자리! :" + this);
            if (TotalHeatUI != null) TotalHeatUI.text = heatPoint.ToString();
        }
    }

    #endregion

    private Renderer render;

    private void Awake()
    {
        render = GetComponentInChildren<Renderer>();
        influencingUnit = new List<PlaceObserver>();
    }

    private void ChangeEffect(float intencity)
    {
        effect.ChangeIntencity(intencity);
    }

    private void Start()
    {

        switch (type)
        {
            case PlaceType.A:
                typeColor = Color.white;
                break;
            case PlaceType.B:
                typeColor = Color.black;
                break;
            case PlaceType.V:
                typeColor = Color.green;
                break;
        }

        render.material.color = typeColor;
    }

    private void OnMouseUpAsButton()
    {
        if(GameManager.Instance.curPlayer is AI)
        {
            Debug.Log("AI의 차례임");
            return;
        }

        if(PlaceManager.Instance.SelectedPiece == null)
        {
            Debug.Log("선택된 기물 없음");
            return;
        }


        if(GameManager.Instance.curState is StatePreparingGame)
        {
            Debug.Log("배치 상태 위치 클릭 진입");
            if(IsMovableToCurPiece)
            {
                PlaceManager.Instance.MovePiece(PlaceManager.Instance.SelectedPiece, this);
                PlaceManager.Instance.SelectedPieceInit();
                return;
            }
        }
        else if(GameManager.Instance.curState is StateOnTurn)
        {
            StateOnTurn stateOnTurn = GameManager.Instance.curState as StateOnTurn;
            if(!GameManager.Instance.playerValidToSelectPlace)
            //if (stateOnTurn.DecideFinished)
            {
                Debug.Log("이미 움직임 결정됨");
                return;
            }
            if (piece != null)
            {
                // 삭제 예정
                Debug.Log("자리에 기물이 있음 (공격을 원할시 기물을 눌러야 함)");
                return;
            }

            PlaceManager.Instance.MoveProcess(PlaceManager.Instance.SelectedPiece, this);
        }
    }

    public void AddInfluence(TeamData.Direction type)
    {
        switch (type)
        {
            case TeamData.Direction.UpToDown:
                HeatPointTopTeam++;
                break;
            case TeamData.Direction.DownToUp:
                HeatPointBottomTeam++;
                break;
            default:
                break;
        }
    }
    public void SubInfluence(TeamData.Direction type)
    {
        switch (type)
        {
            case TeamData.Direction.UpToDown:
                HeatPointTopTeam--;
                break;
            case TeamData.Direction.DownToUp:
                HeatPointBottomTeam--;
                break;
            default:
                break;
        }
    }
    public void ChangeColor(Color color)
    {
        render.material.color = color;
    }

    public void ChangeColor()
    {
        render.material.color = typeColor;
    }

    public void BeEmpty()
    {
        piece = null;
    }

    public void BeFilled(Piece piece)
    {
        this.piece = piece;
    }


    public void registerObserver(IObserver observer)
    {
        influencingUnit.Add(observer as PlaceObserver);
    }

    public void removeObserver(IObserver observer)
    {
        influencingUnit.Remove(observer as PlaceObserver);
    }

    public ISubject notifyObserver()
    {
        // 옵저버에게 알리는 과정에서, 옵저버 목록이 바뀔 수 있다.
        // 현재 목록대로 옵저버에게 알려야하므로, 복사본을 만들어야 한다.

        List<IObserver> copyList = new List<IObserver>();

        for (int i = 0; i < influencingUnit.Count; i++)
        {
            copyList.Add(influencingUnit[i]);
        }

        for (int i = 0; i < copyList.Count; i++)
        {
            copyList[i].StateUpdate(this);
        }

        return this;
    }

    public ITargetable.Type React()
    {
        // 예외처리: 위에 기물이 있는 경우
        if (piece != null)
            return ITargetable.Type.Attack;
        else
            return ITargetable.Type.Peace;
    }
}
