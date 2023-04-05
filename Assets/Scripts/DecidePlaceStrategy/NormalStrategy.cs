using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStrategy : DecidePlaceStrategy
{
    public override Placement DecidePlace(Piece piece, ref float will, out ScoreNode scoreSet)
    {
        return base.DecidePlace(piece, ref will, out scoreSet);
    }
}
