using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggresiveStrategy : MonoBehaviour, IAIStrategy
{
    SortedList<float, Placement> possibilities;
    public void AddPossibility(ScoreNode scoreSet, Piece piece, Place place)
    {
        // 스코어 노드를 받아서

        // 자체적으로 계산

        // 소티드 리스트에 넣는다.
        Placement placement = new Placement(piece, piece.place, place, place.Piece);
    }

    public Placement GetBestInOwnWay()
    {
        // 소티드 리스트에서 첫번째 혹은 다른 것을 가져온다.

        return null;
    }
}
