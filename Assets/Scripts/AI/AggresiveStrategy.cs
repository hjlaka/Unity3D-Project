using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggresiveStrategy : AIStrategy
{
    SortedList<float, Placement> possibilities;
    public override void AddPossibility(ScoreNode scoreSet, Piece piece, Place place)
    {
        // ���ھ� ��带 �޾Ƽ�

        // ��ü������ ���

        // ��Ƽ�� ����Ʈ�� �ִ´�.
        Placement placement = new Placement(piece, piece.place, place, place.Piece, null);
    }

    public override Placement GetBestInOwnWay()
    {
        // ��Ƽ�� ����Ʈ���� ù��° Ȥ�� �ٸ� ���� �����´�.

        return null;
    }
}
