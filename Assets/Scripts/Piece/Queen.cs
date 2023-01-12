using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    protected override void Awake()
    {
        base.Awake();

        movePattern = new EightWayMove(this);
    }
}
