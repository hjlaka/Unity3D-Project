using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private PlaceEffect effect;

    [SerializeField]
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

            //if(heatPoint < 0) heatPoint = 0;
            if(effect != null)
                effect.Intencity = heatPoint * 0.2f;


            if (heatPoint >= 3) Debug.Log(heatPoint + " ������ �ڸ�! :" + this);
        }
    }

    public Vector2Int boardIndex;

    private Renderer render;
    private bool isApprochable;
    public bool IsApprochable { get { return isApprochable; } set { isApprochable = value; } }


    public PlaceType type;     // ��ũ���ͺ� ������Ʈ

    private Color typeColor;

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
        //Debug.Log(string.Format("{0} Ŭ��", gameObject.name));

        // �⹰�� ���õ� ���¿��� Ŭ����

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
