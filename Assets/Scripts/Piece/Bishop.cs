using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{

    public override void PieceAction()
    {
        base.PieceAction();

    }
    public override bool IsMovable(Vector2Int location)
    {
        if(Mathf.Abs(location.x - place.boardIndex.x) == Mathf.Abs(location.y - place.boardIndex.y))
        {
            //Debug.Log("[" + (location.x - place.boardIndex.x) + "][" + (location.y - place.boardIndex.y) + "]");
            return true;
        }
            
        else
            return false;
    }
}
