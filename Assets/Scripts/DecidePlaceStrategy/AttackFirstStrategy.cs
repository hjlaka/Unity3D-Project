using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFirstStrategy : DecidePlaceStrategy
{
    public AttackFirstStrategy()
    {
        strategyData = StrategyManager.Instance.attackFirst;
    }
    public override Placement DecidePlace(Piece piece, ref float will, out ScoreNode scoreSet)
    {
       CopyData();
       return base.DecidePlace(piece, ref will, out scoreSet);
    }
}
