using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Place : MonoBehaviour
{
    public enum PlaceType { A, B};
    public Piece piece;
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
        set { heatPointTopTeam = value; HeatPoint = heatPointBottomTeam + heatPointTopTeam;
            //Debug.Log(string.Format("위팀 과열도 {0}, 전체 과열도 {1}", heatPointTopTeam, heatPoint));
            if(topTeamHeatUI != null) topTeamHeatUI.text = heatPointTopTeam.ToString();
        }
    }

    public int HeatPointBottomTeam
    {
        get { return heatPointBottomTeam; }
        set { heatPointBottomTeam = value; HeatPoint = heatPointBottomTeam + heatPointTopTeam;
            //Debug.Log(string.Format("아래팀 과열도 {0}, 전체 과열도 {1}", heatPointBottomTeam, heatPoint));
            if (bottomTeamHeatUI != null)  bottomTeamHeatUI.text = heatPointBottomTeam.ToString();
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

            if(heatPoint < 0) heatPoint = 0;
            if(effect != null)
                effect.Intencity = heatPoint * 0.2f;


            if (heatPoint >= 3) Debug.Log(heatPoint + " 과열된 자리! :" + this);
            if (TotalHeatUI != null)  TotalHeatUI.text = heatPoint.ToString();
        }
    }

    #endregion



    private Renderer render;
    private bool isApprochable;
    public bool IsApprochable { get { return isApprochable; } set { isApprochable = value; } }




    private void Awake()
    {
        render = GetComponentInChildren<Renderer>();
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
            case PlaceType.A: typeColor = Color.white;
                break;
            case PlaceType.B: typeColor = Color.black;
                break;
        }

        render.material.color = typeColor;
    }

    private void OnMouseDown()
    {
        //Debug.Log(string.Format("{0} 클릭", gameObject.name));


        // 게임 상태 조건
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PLACE)
            return;

        // 기물이 선택된 상태에서 클릭시
        if (null == PlaceManager.Instance.SelectedPiece)
            return;

        if (piece != null)
            return;

        if (!isApprochable)
            return;

        PlaceManager.Instance.MovePieceTo(PlaceManager.Instance.SelectedPiece, this);


    }

    public void ChangeColor(Color color)
    {
        render.material.color = color;
    }

    public void ChangeColor()
    {
        render.material.color = typeColor;
    }


}
