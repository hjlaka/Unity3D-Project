using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStrategy : MonoBehaviour
{
    public enum AIStrategyType { NONE, AGGRESIVE, RANDOM, WILLFIRST }

    public abstract void ClearPossibility();
    public abstract void AddPossibility(ScoreNode scoreSet, Piece piece, Place place);

    public abstract Placement GetBestInOwnWay();
}
