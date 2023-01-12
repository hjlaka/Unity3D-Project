using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightWayMove : MoveRecognizer, IPieceMovable
{
    IPieceMovable straight;
    IPieceMovable diagonal;

    public EightWayMove(Piece controlled) : base(controlled)
    {
        straight = new StraightMove(controlled);
        diagonal = new DiagonalMove(controlled);

    }
    public void RecognizeRange(Vector2Int location)
    {
        straight.RecognizeRange(location);
        diagonal.RecognizeRange(location);
        // ��������� ��Ұ� �ߺ��Ǵ� ���� ���� ��ó�� ����.
    }

    public void RecognizeSpecialMove(Place newPlace)
    {
        // Do Nothing
    }
}
