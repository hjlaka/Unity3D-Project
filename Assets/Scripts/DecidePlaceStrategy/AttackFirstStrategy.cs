using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFirstStrategy : IDecidePlaceStrategy
{
    public Place DecidePlace(Piece piece)
    {
        Place place = null;
        int attackPoint = 0;

        // 데이터에는 결함이 없는 상태로 가정한다.
        List<Place> movablePlaces = piece.MovableTo;
        List<Piece> attackablePieces = piece.ThreatTo;

        Debug.Log("공격 우선 선택");

        Debug.Log("공격 가능 수: " + attackablePieces.Count);
        // 공격 가능 확인
        if (attackablePieces.Count > 0)
        {
            int heatPoint = 99;
            for (int i = 0; i < attackablePieces.Count; i++)
            {

                // 과열도가 낮은 곳 우선
                if (attackablePieces[i].place.HeatPoint <= heatPoint)
                {
                    place = attackablePieces[i].place;
                    heatPoint = attackablePieces[i].place.HeatPoint;
                }
            }

            return place;
        }
        
        
       

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            Debug.Log(string.Format("과열도 : {0}, 현재 포인트 : {1}", movablePlaces[i].HeatPoint, attackPoint));
            if (movablePlaces[i].HeatPoint >= attackPoint)
            {
                place = movablePlaces[i];
                attackPoint = movablePlaces[i].HeatPoint;
            }
        }

        return place;
    }
}
