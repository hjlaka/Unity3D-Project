using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDecidePlaceStrategy
{
    public enum StategyType { AttackFirst, DefenceFirst, SaftyFirst, Normal }
    Placement DecidePlace(Piece piece, ref float will, out ScoreNode scoreSet);
}
