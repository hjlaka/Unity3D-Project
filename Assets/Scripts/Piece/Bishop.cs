using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    DiagonalMover diagonalMover;

    protected override void Awake()
    {
        base.Awake();

        diagonalMover = GetComponent<DiagonalMover>();
    }
    public override void RecognizeRange(Vector2Int location)
    {

        diagonalMover.CheckRange(place.board.Size, location);

    }

    


}
