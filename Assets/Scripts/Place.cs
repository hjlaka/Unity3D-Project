using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    public enum PlaceType { A, B};
    public Piece piece;

    private Renderer render;


    public PlaceType type;     // ��ũ���ͺ� ������Ʈ

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
        //Debug.Log(string.Format("{0} Ŭ��", gameObject.name));

        // �⹰�� ���õ� ���¿��� Ŭ����

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
    // �⹰ ���� ��Ȳ����

    // �⹰�� �ִ� ���
    // �⹰ ����
    //}

}
