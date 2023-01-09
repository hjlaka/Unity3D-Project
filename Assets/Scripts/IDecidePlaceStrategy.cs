using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDecidePlaceStrategy
{
    void DecidePlace(List<Place> movablePlaces);
}
