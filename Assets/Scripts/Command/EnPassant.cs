using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnPassant : SpecialMove
{
    private Piece subject;
    private Place targetPlace;
    // 발동 조건 체크


    // 발동

    public override Placement DoAction()
    {
        PlaceManager.Instance.Attack(subject, targetPlace.Piece);

        Placement result = new Placement(null, targetPlace, null, targetPlace.Piece, null);
        return result;
    }

    public override bool Validate()
    {
        throw new System.NotImplementedException();
    }
}
