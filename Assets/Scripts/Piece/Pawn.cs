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


    public override Piece SetInPlace(Place place)
    {
        movePattern.RecognizeSpecialMove(place);
        // TODO: �������̵� ���ص� �� ���� ����

        return base.SetInPlace(place);
    }

}
