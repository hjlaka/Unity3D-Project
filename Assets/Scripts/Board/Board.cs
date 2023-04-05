using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Place[,] places;
    private Vector2Int size;
    public Vector2Int Size
    {
        get { return size; }
        set { size = value; }
    }

    private bool followRule;
    public bool FollowRule 
    { 
        get { return followRule; } 
        set { followRule = value; } 
    }

    public Place GetPlace(Vector2Int index)
    {
        if (index.x > size.x || index.y > size.y) return null;

        return places[index.x, index.y];
    }

    public void ApplyMovable(Piece piece, bool value)
    {
        List<Place> movablePlaces = piece.Recognized.movable;
        for(int i = 0; i < movablePlaces.Count; i++)
        {
            movablePlaces[i].IsMovableToCurPiece = value;
        }
    }
}
