using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용하지 않을 예정.
public interface IPieceMovable
{
    void RecognizeRange(Vector2Int location, StateLists recognized);

    void RecognizeSpecialMove(Place newPlace);
}
