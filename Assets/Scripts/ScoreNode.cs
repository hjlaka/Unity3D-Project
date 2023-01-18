using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreNode
{
    private float attackPoint;
    public float AttackPoint { get { return attackPoint; } set { attackPoint = value; } }
    private float defencdPoint;
    public float DefencdPoint { get { return defencdPoint; } set { attackPoint = value; } }
    private float influencePoint;
    public float InfluencePoint { get { return influencePoint; } set { attackPoint = value; } }
    private float totalPoint;
    public float TotalPoint { get { return totalPoint = attackPoint + defencdPoint + influencePoint; } set { attackPoint = value; } }

    public void Clear()
    {
        attackPoint = 0;
        defencdPoint = 0;
        influencePoint = 0;
        totalPoint = 0;
    }
}
