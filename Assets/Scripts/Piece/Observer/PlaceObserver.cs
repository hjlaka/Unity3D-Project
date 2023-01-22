using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaceObserver : MonoBehaviour, IObserver
{
    private Piece piece;
    private void Awake()
    {
        piece = GetComponent<Piece>();
    }
    public void StateUpdate()
    {
        PlaceManager.Instance.ReCalculateInfluence(piece);
    }
}
