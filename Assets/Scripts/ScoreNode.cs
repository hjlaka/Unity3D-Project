using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreNode
{
    private float attackPoint;
    public float AttackPoint { get { return attackPoint; } set { attackPoint = value; } }

    private float defencdPoint;
    public float DefencdPoint { get { return defencdPoint; } set { defencdPoint = value; } }

    private float influencePoint;
    public float InfluencePoint { get { return influencePoint; } set { influencePoint = value; } }

    private float extendPoint;
    public float ExtendPoint { get { return extendPoint; } set { extendPoint = value; } }

    private float deltaHeatPoint;
    public float DeltaHeatPoint { get { return deltaHeatPoint; } set { deltaHeatPoint = value; } }

    private float willPoint;
    public float WillPoint { get { return willPoint; } set { willPoint = value; } }



    public void Clear()
    {
        attackPoint = 0;
        defencdPoint = 0;
        influencePoint = 0;
        deltaHeatPoint = 0;
    }
}
