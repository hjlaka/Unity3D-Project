using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaftyFirstStrategy : IDecidePlaceStrategy
{

    // �� ���������� �⹰�� �������� ���� ���� �ִٰ� �����Ѵ�.
    public Placement DecidePlace(Piece piece, ref float will, out ScoreNode scoreSet)
    {

        /* Place place = null;


         // �����Ϳ��� ������ ���� ���·� �����Ѵ�.
         List<Place> movablePlaces = piece.Recognized.movable;
         List<Piece> attackablePieces = piece.Recognized.threating;

         Debug.Log("���� �켱 ����");

         // ������ ���� ���ݵ� �� ���� �ʴ´�.





         // ���� ���� Ȯ��
         if (attackablePieces.Count > 0)
         {
             int teamPoint = 0;
             int heatPoint = 99;

             for (int i = 0; i < attackablePieces.Count; i++)
             {

                 Place checkingPlace = attackablePieces[i].place;
                 int teamHeat = piece.returnHeat.ReturnTeamHeat(checkingPlace);
                 int oppenentHeat = piece.returnHeat.ReturnOpponentHeat(checkingPlace);

                 int checkingTeamPoint = teamHeat - oppenentHeat;

                 if (teamPoint > checkingTeamPoint) continue;       // �Ʊ� �������� �� ���ƾ߸� ���

                 // �������� �� ���� �� �켱 (���� ����)
                 if (heatPoint < checkingPlace.HeatPoint) continue;

                 place = attackablePieces[i].place;
                 teamPoint = checkingTeamPoint;

             }

             if(place != null)
                 return place;
         }


         // ������ ���� �������� ���� ���

         int riskPoint = 0;

         for (int i = 0; i < movablePlaces.Count; i++)
         {
             Place checkingPlace = movablePlaces[i];

             int teamHeat = piece.returnHeat.ReturnTeamHeat(checkingPlace);
             int oppenentHeat = piece.returnHeat.ReturnOpponentHeat(checkingPlace);

             // ���赵�� ������ �� ������ ������ ����.
             int checkingRiskPoint = oppenentHeat - teamHeat;

             if (riskPoint >= checkingRiskPoint)
             {
                 //Debug.Log(string.Format("{0} �ڸ��� ���赵�� {1} �̹Ƿ�, {2} ���� �ڸ����� �����ϰڴ�.", movablePlaces[i].boardIndex, checkingRiskPoint, heatPoint));
                 place = movablePlaces[i];
                 riskPoint = checkingRiskPoint;
             }
         }

         return place;*/
        scoreSet = null;

        return null;
    }

}
