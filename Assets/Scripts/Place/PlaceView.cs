using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceView : MonoBehaviour
{
    // �÷��̾�� ����
    private Renderer render;

    private void Awake()
    {
        render = GetComponentInChildren<Renderer>();
    }

    public void ShowPlaceState(/* Ÿ�� �� */)
    {
        // Ÿ�� ���� ���� �� ���� Ȥ�� ǥ�� ����
    }

    public void ChangeColor(Color color)
    {
        render.material.color = color;
    }

    private void OnMouseUpAsButton()
    {

    }

}
