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
        // 스코어 노드를 받아서

        // 자체적으로 계산
        scoreSet.WillPoint *= willBonous;
        // 소티드 리스트에 넣는다.
        Placement placement = new Placement(piece, piece.place, place, place.Piece, null);

        possibilities.Push(new Node<Placement>(scoreSet.WillPoint, placement));
    }

    public override Placement GetBestInOwnWay()
    {
        // 소티드 리스트에서 첫번째 혹은 다른 것을 가져온다.

        return possibilities.Pop().obj;
    }

    public override void ClearPossibility()
    {
        possibilities.Clear();
    }
}
