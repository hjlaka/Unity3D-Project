using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{

    protected override void Awake()
    {
        base.Awake();
        movePattern = new PawnMove(this);
    }

    public override void RecognizeRange(Vector2Int location)
    {
        movePattern.RecognizeRange(location);
        // TODO: �������̵� ���ص� �� ���� ����
    }

    public override void SetInPlace(Place place)
    {
        movePattern.RecognizeSpecialMove(place);
        // TODO: �������̵� ���ص� �� ���� ����

        base.SetInPlace(place);
    }

}
