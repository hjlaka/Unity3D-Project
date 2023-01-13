using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{

    protected override void Awake()
    {
        base.Awake();
        movePattern = new PawnMove(this);

        pieceScore = 1;
    }


    public override void SetInPlace(Place place)
    {
        movePattern.RecognizeSpecialMove(place);
        // TODO: 오버라이드 안해도 될 수도 있음

        base.SetInPlace(place);
    }

}
