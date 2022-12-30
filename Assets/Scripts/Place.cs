using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    public enum PlaceType { A, B};
    public Piece piece;

    private Renderer render;


    public PlaceType type;     // 스크립터블 오브젝트

    private Color typeColor;

    private void Awake()
    {
        render = GetComponent<Renderer>();
    }

    private void Start()
    {
        switch(type)
        {
            case PlaceType.A: typeColor = Color.white;
                break;
            case PlaceType.B: typeColor = Color.black;
                break;
        }
    }

    private void OnMouseDown()
    {
        //Debug.Log(string.Format("{0} 클릭", gameObject.name));

        // 기물이 선택된 상태에서 클릭시

        if (null == PlaceManager.Instance.selectedPiece)
            return;

        if (piece != null)
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


    //{
    // 기물 선택 상황에서

    // 기물이 있는 경우
    // 기물 선택
    //}

}
