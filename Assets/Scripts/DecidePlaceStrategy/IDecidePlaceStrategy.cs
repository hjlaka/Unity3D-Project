using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDecidePlaceStrategy
{
    public enum StategyType { AttackFirst, DefenceFirst, SaftyFirst }
    Place DecidePlace(Piece piece, ref float will);
}
