using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{

    protected override void Awake()
    {
        base.Awake();

        movePattern = new DiagonalMove(this);
    }

}
