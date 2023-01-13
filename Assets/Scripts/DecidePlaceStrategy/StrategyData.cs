using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chess/Strategy")]
public class StrategyData : ScriptableObject
{
    public float willingToDefend = 1f;
    public float willingToAttack = 4f;
    public float willingToThreat = 1.2f;
    public float willingToExtend = 0.5f;
    public float willingToSafe = 1.3f;
    public float futureOriented = 0.1f;
}
