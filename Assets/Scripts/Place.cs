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
    public PlaceType type;     // ��ũ���ͺ� ������Ʈ
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

    #region ������Ƽ
    public int HeatPointTopTeam
    {
        get { return heatPointTopTeam; }
        set
        {
            heatPointTopTeam = value; HeatPoint = heatPointBottomTeam + heatPointTopTeam;
            //Debug.Log(string.Format("���� ������ {0}, ��ü ������ {1}", heatPointTopTeam, heatPoint));
            if (topTeamHeatUI != null) topTeamHeatUI.text = heatPointTopTeam.ToString();
        }
    }

    public int HeatPointBottomTeam
    {
        get { return heatPointBottomTeam; }
        set
        {
            heatPointBottomTeam = value; HeatPoint = heatPointBottomTeam + heatPointTopTeam;
            //Debug.Log(string.Format("�Ʒ��� ������ {0}, ��ü ������ {1}", heatPointBottomTeam, heatPoint));
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


            //if (heatPoint >= 3) Debug.Log(heatPoint + " ������ �ڸ�! :" + this);
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
            Debug.Log("AI�� ������");
            return;
        }

        if(PlaceManager.Instance.SelectedPiece == null)
        {
            Debug.Log("���õ� �⹰ ����");
            return;
        }


        if(GameManager.Instance.curState is StatePreparingGame)
        {
            Debug.Log("��ġ ���� ��ġ Ŭ�� ����");
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
                Debug.Log("�̹� ������ ������");
                return;
            }
            if (piece != null)
            {
                // ���� ����
                Debug.Log("�ڸ��� �⹰�� ���� (������ ���ҽ� �⹰�� ������ ��)");
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
        // ���������� �˸��� ��������, ������ ����� �ٲ� �� �ִ�.
        // ���� ��ϴ�� ���������� �˷����ϹǷ�, ���纻�� ������ �Ѵ�.

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
        // ����ó��: ���� �⹰�� �ִ� ���
        if (piece != null)
            return ITargetable.Type.Attack;
        else
            return ITargetable.Type.Peace;
    }
}
