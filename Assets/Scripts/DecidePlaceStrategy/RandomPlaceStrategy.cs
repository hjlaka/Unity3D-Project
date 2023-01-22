using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlaceStrategy : DecidePlaceStrategy
{
    public override Placement DecidePlace(Piece piece, ref float will, out ScoreNode scoreset)
    {

        List<Place> movablePlaces = piece.Recognized.movable;

        int randIndex = Random.Range(0, movablePlaces.Count);

        will = 0.7f;
        scoreset = new ScoreNode();

        Placement placement = new Placement(piece, piece.place, movablePlaces[randIndex], movablePlaces[randIndex].Piece, null);

        return placement;
    }

}
