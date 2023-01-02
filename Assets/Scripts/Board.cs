using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Vector2Int size;
    public Vector2Int Size
    {
        get { return size; }
        set { size = value; }
    }

    private bool followRule;
    public bool FollowRule { get; set; }
    public Place[,] places;
}
