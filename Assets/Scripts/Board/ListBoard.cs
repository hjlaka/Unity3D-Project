using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListBoard : Board
{
    [SerializeField]
    private List<Place> placeList = new List<Place>();

    public Place AutoAddPiece(Piece piece)
    {
        for(int i = 0; i < placeList.Count; i++)
        {
            if (placeList[i].Piece == null)
            {
                placeList[i].Piece = piece;
                return placeList[i];
            }  
        }

        return null;
    }
}
