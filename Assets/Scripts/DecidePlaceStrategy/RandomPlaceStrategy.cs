using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlaceStrategy : DecidePlaceStrategy, IDecidePlaceStrategy
{
    public Place DecidePlace(Piece piece)
    {

        List<Place> movablePlaces = piece.Recognized.movable;

        int randIndex = Random.Range(0, movablePlaces.Count);

        return movablePlaces[randIndex];
    }

}
