using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPieceMovable
{
    void RecognizeRange(Vector2Int location);

    void RecognizeSpecialMove(Place newPlace);
}
