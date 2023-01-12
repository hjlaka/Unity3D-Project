using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    protected override void Awake()
    {
        base.Awake();

        movePattern = new JumpMove(this);
    }
    public override void RecognizeRange(Vector2Int location)
    {
        movePattern.RecognizeRange(location);
    }



}
