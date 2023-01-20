using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlaceStrategy : DecidePlaceStrategy, IDecidePlaceStrategy
{
    public Place DecidePlace(Piece piece, ref float will, out ScoreNode scoreset)
    {

        List<Place> movablePlaces = piece.Recognized.movable;

        int randIndex = Random.Range(0, movablePlaces.Count);

        will = 0.7f;
        scoreset = new ScoreNode();

        return movablePlaces[randIndex];
    }

}
