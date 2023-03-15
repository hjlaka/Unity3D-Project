using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightWayMove : MoveRecognizer, IPieceMovable
{
    IPieceMovable straight;
    IPieceMovable diagonal;

    public EightWayMove(Piece controlled, int level) : base(controlled)
    {
        straight = new StraightMove(controlled, level);
        diagonal = new DiagonalMove(controlled, level);

    }
    public void RecognizeRange(Vector2Int location, StateLists recognized)
    {
        straight.RecognizeRange(location, recognized);
        diagonal.RecognizeRange(location, recognized);
        // 현재까지는 장소가 중복되는 데에 대한 대처가 없다.
    }

    public void RecognizeSpecialMove(Place newPlace)
    {
        // Do Nothing
    }
}
