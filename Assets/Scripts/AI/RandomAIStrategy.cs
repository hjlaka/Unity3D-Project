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
        // piece�� place
        Debug.Log(string.Format("�⹰ {0} �ڸ� {1}", piece, place));
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
