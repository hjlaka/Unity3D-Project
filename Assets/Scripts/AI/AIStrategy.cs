using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStrategy : MonoBehaviour
{
    public enum AIStrategyType { AGGRESIVE, RANDOM }
    public abstract void AddPossibility(ScoreNode scoreSet, Piece piece, Place place);

    public abstract Placement GetBestInOwnWay();
}
