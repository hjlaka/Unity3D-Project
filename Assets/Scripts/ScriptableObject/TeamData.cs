using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chess/Team")]
public class TeamData : ScriptableObject
{
    public string TeamName;
    public uint TeamId;
    public Color normal;
}
