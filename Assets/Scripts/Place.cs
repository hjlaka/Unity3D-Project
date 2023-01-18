using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Place : MonoBehaviour, ISubject
{
    public enum PlaceType { A, B };
    private Piece piece;
    public Piece Piece
    {
        get { return piece; }
        set { piece = value; }
    }
    public Board board;
    public Vector2Int boardIndex;
    public PlaceType type;     // 스크립터블 오브젝트

    private Color typeColor;


    [Header("Running Game")]

    [SerializeField]
    private bool isMovableToCurPiece = false;
    [SerializeField]
    private bool isAttackableByCurPiece = false;

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

    [SerializeField]
    private List<Piece> influencingUnit; // 디버그 위해 변환

    [SerializeField]
    private PlaceEffect effect;

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
    private bool isApprochable;
    public bool IsApprochable { get { return isApprochable; } set { isApprochable = value; } }




    private void Awake()
    {
        render = GetComponentInChildren<Renderer>();
        influencingUnit = new List<Piece>();
    }

    private void ChangeEffect(float intencity)
    {
        effect.ChangeIntencity(intencity);
    }

    private void Start()
    {
        IsApprochable = true;

        switch (type)
        {
            case PlaceType.A:
                typeColor = Color.white;
                break;
            case PlaceType.B:
                typeColor = Color.black;
                break;
        }

        render.material.color = typeColor;
    }

    private void OnMouseUpAsButton()
    {
        //if (!GameManager.Instance.isPlayerTurn) { Debug.Log("플레이어 턴 아님"); return; }
        //Debug.Log(string.Format("{0} 클릭", gameObject.name));


        // 게임 상태 조건
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PLACE)
        {
            Debug.Log("위치 선택 가능 상태 아님");
            return;
        }


        // 기물이 선택된 상태에서 클릭시
        if (null == PlaceManager.Instance.SelectedPiece)
        {
            Debug.Log("선택된 기물이 없음");
            return;
        }

        if (piece != null)
        {
            Debug.Log("자리에 기물이 있음 (공격을 원할시 기물을 눌러야 함)");
            return;
        }

        if (!isApprochable)
        {
            Debug.Log("접근 불가능한 위치임");
            return;
        }


        PlaceManager.Instance.MoveProcess(PlaceManager.Instance.SelectedPiece, this);


    }

    /*    public void UpdateInfluencingPieces()
        {
            for(int i = 0; i < influencingPieces.Count; i++)
            {
                Piece piece = influencingPieces[i];
                PlaceManager.Instance.ReCalculateInfluence(piece);
            }
        }*/

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
        notifyObserver();
    }

    public void BeFilled(Piece piece)
    {
        this.piece = piece;

        if (piece.team.direction == TeamData.Direction.DownToUp)
            HeatPointBottomTeam++;
        else
            HeatPointTopTeam++;

        notifyObserver();
    }


    public void registerObserver(IObserver observer)
    {
        influencingUnit.Add(observer as Piece);
    }

    public void removeObserver(IObserver observer)
    {
        influencingUnit.Remove(observer as Piece);
    }

    public void notifyObserver()
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
            copyList[i].StateUpdate();
            Debug.Log("업데이트 호출: " + copyList[i]);
        }
    }
}
