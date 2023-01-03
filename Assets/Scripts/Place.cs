using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    public enum PlaceType { A, B};
    public Piece piece;
    public Board board;

    private int heatPoint;
    public int HeatPoint
    {
        get
        { 
            return heatPoint; 
        }
        set
        {
            heatPoint = value;
            //Debug.Log(boardIndex + "의 과열도 증가: " + value);
        }
    }

    public Vector2Int boardIndex;

    private Renderer render;
    private bool isApprochable;
    public bool IsApprochable { get { return isApprochable; } set { isApprochable = value; } }


    public PlaceType type;     // 스크립터블 오브젝트

    private Color typeColor;

    private void Awake()
    {
        render = GetComponentInChildren<Renderer>();
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

        // 기물이 선택된 상태에서 클릭시

        if (null == PlaceManager.Instance.SelectedPiece)
            return;

        if (piece != null)
            return;

        if (!isApprochable)
            return;

        PlaceManager.Instance.MovePieceTo(this);


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
