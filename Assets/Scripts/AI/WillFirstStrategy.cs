using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillFirstStrategy : AIStrategy
{
    private float willBonous = 2f;
    Heap<Placement> possibilities;

    private void Awake()
    {
        this.possibilities = new Heap<Placement>();
    }
    public override void AddPossibility(ScoreNode scoreSet, Piece piece, Place place)
    {
        // ���ھ� ��带 �޾Ƽ�

        // ��ü������ ���
        scoreSet.WillPoint *= willBonous;
        // ��Ƽ�� ����Ʈ�� �ִ´�.
        Placement placement = new Placement(piece, piece.place, place, place.Piece, null);

        possibilities.Push(new Node<Placement>(scoreSet.WillPoint, placement));
    }

    public override Placement GetBestInOwnWay()
    {
        // ��Ƽ�� ����Ʈ���� ù��° Ȥ�� �ٸ� ���� �����´�.

        return possibilities.Pop().obj;
    }

    public override void ClearPossibility()
    {
        possibilities.Clear();
    }
}
