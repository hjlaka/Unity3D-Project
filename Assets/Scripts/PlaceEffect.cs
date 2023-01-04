using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceEffect : MonoBehaviour
{
    private Renderer render;

    private float intencity;
    public float Intencity
    {
        get 
        { 
            return intencity; 
        } 
        set 
        {
            //Debug.Log("ºÒ¸®³ª?");
            intencity = value; 
            ChangeIntencity(value); 
        }
    }

    private void Awake()
    {
        render = GetComponent<Renderer>();
        intencity = 0f;
        ChangeIntencity(0f);

    }
    public void ChangeIntencity(float intencity)
    {
        Color curColor = render.material.color;
        curColor.a = intencity;
        render.material.color = curColor;
    }

    public void ChangeColor(Color color)
    {
        Color newColor = color;
        newColor.a = intencity;
        render.material.color = newColor;
    }
}
