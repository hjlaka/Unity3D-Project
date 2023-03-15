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
        // ��������� ��Ұ� �ߺ��Ǵ� ���� ���� ��ó�� ����.
    }

    public void RecognizeSpecialMove(Place newPlace)
    {
        // Do Nothing
    }
}
