using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightWayMove : MoveRecognizer, IPieceMovable
{
    IPieceMovable straight;
    IPieceMovable diagonal;
    //private Piece controlled;

    public EightWayMove(Piece controlled)
    {
        //this.controlled = controlled;
        straight = new StraightMove(controlled);
        diagonal = new DiagonalMove(controlled);

    }
    public void RecognizeRange(Vector2Int location)
    {
        straight.RecognizeRange(location);
        diagonal.RecognizeRange(location);
        // 현재까지는 장소가 중복되는 데에 대한 대처가 없다.
    }

    public void RecognizeSpecialMove(Place newPlace)
    {
        // Do Nothing
    }
}
