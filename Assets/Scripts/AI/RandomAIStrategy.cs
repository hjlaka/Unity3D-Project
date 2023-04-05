using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAIStrategy : AIStrategy
{

    private List<Placement> possibilities;
    private void Awake()
    {
        possibilities = new List<Placement>();
    }
    public override void AddPossibility(ScoreNode scoreSet, Piece piece, Place place)
    {
        // piece와 place
        Debug.Log(string.Format("기물 {0} 자리 {1}", piece, place));
        possibilities.Add(new Placement(piece, piece.place, place, place.Piece, null));
    }

    public override Placement GetBestInOwnWay()
    {
        if (possibilities.Count <= 0) 
            return null;

        return possibilities[Random.Range(0, possibilities.Count - 1)];
    }

    public override void ClearPossibility()
    {
        possibilities.Clear();
    }
}
