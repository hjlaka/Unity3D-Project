using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chess/Team")]
public class TeamData : ScriptableObject
{
    public enum Direction { DownToUp, UpToDown };

    [Header("General")]
    public string TeamName;
    public uint TeamId;
    public Color normal;

    [Header("Setting")]
    public Direction direction;

}
