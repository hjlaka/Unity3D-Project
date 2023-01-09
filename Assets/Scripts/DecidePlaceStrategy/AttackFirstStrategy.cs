using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFirstStrategy : IDecidePlaceStrategy
{
    public Place DecidePlace(Piece piece)
    {
        Place place = null;
        int attackPoint = 0;

        // �����Ϳ��� ������ ���� ���·� �����Ѵ�.
        List<Place> movablePlaces = piece.MovableTo;
        List<Piece> attackablePieces = piece.ThreatTo;

        Debug.Log("���� �켱 ����");

        Debug.Log("���� ���� ��: " + attackablePieces.Count);
        // ���� ���� Ȯ��
        if (attackablePieces.Count > 0)
        {
            int heatPoint = 99;
            for (int i = 0; i < attackablePieces.Count; i++)
            {

                // �������� ���� �� �켱
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
            Debug.Log(string.Format("������ : {0}, ���� ����Ʈ : {1}", movablePlaces[i].HeatPoint, attackPoint));
            if (movablePlaces[i].HeatPoint >= attackPoint)
            {
                place = movablePlaces[i];
                attackPoint = movablePlaces[i].HeatPoint;
            }
        }

        return place;
    }
}
