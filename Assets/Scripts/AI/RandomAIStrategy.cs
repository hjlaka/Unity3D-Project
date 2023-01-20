using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAIStrategy : MonoBehaviour, IAIStrategy
{

    private List<Placement> possibilities;
    public void AddPossibility(ScoreNode scoreSet, Piece piece, Place place)
    {
        // piece¿Í place
        possibilities.Add(new Placement(piece, piece.place, place, place.Piece));
    }

    public Placement GetBestInOwnWay()
    {
        if (possibilities.Count <= 0) 
            return null;

        return possibilities[Random.Range(0, possibilities.Count - 1)];
    }
}
