using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceView : MonoBehaviour
{
    // 플레이어에게 갱신
    private Renderer render;

    private void Awake()
    {
        render = GetComponentInChildren<Renderer>();
    }

    public void ShowPlaceState(/* 타입 값 */)
    {
        // 타입 값에 따라 색 변경 혹은 표시 변경
    }

    public void ChangeColor(Color color)
    {
        render.material.color = color;
    }

    private void OnMouseUpAsButton()
    {

    }

}
