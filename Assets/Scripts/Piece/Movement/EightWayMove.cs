using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightWayMove : MoveRecognizer
{
    readonly MoveRecognizer straight;
    readonly MoveRecognizer diagonal;

    public EightWayMove(Piece controlled, int level) : base(controlled)
    {
        straight = new StraightMove(controlled, level);
        diagonal = new DiagonalMove(controlled, level);

    }
    public override void RecognizeRange(Vector2Int location, StateLists recognized)
    {
        straight.RecognizeRange(location, recognized);
        diagonal.RecognizeRange(location, recognized);
        // ��������� ��Ұ� �ߺ��Ǵ� ���� ���� ��ó�� ����.
    }

    public override void RecognizeSpecialMove(Place newPlace)
    {
        // Do Nothing
    }
}
