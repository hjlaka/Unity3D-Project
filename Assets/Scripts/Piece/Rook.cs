using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    protected override void Awake()
    {
        base.Awake();

        movePattern = new StraightMove(this);
    }
    
}
