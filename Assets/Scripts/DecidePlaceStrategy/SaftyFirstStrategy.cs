using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaftyFirstStrategy : IDecidePlaceStrategy
{

    // 이 전략에서는 기물이 움직이지 않을 수도 있다고 가정한다.
    public Placement DecidePlace(Piece piece, ref float will, out ScoreNode scoreSet)
    {

        /* Place place = null;


         // 데이터에는 결함이 없는 상태로 가정한다.
         List<Place> movablePlaces = piece.Recognized.movable;
         List<Piece> attackablePieces = piece.Recognized.threating;

         Debug.Log("안전 우선 선택");

         // 위험한 곳은 공격도 방어도 하지 않는다.





         // 공격 가능 확인
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

                 if (teamPoint > checkingTeamPoint) continue;       // 아군 과열도가 더 높아야만 고려

                 // 과열도가 더 낮은 곳 우선 (선택 사항)
                 if (heatPoint < checkingPlace.HeatPoint) continue;

                 place = attackablePieces[i].place;
                 teamPoint = checkingTeamPoint;

             }

             if(place != null)
                 return place;
         }


         // 공격할 곳을 선정하지 못한 경우

         int riskPoint = 0;

         for (int i = 0; i < movablePlaces.Count; i++)
         {
             Place checkingPlace = movablePlaces[i];

             int teamHeat = piece.returnHeat.ReturnTeamHeat(checkingPlace);
             int oppenentHeat = piece.returnHeat.ReturnOpponentHeat(checkingPlace);

             // 위험도를 따져서 덜 위험한 곳으로 간다.
             int checkingRiskPoint = oppenentHeat - teamHeat;

             if (riskPoint >= checkingRiskPoint)
             {
                 //Debug.Log(string.Format("{0} 자리가 위험도가 {1} 이므로, {2} 였던 자리보다 안전하겠다.", movablePlaces[i].boardIndex, checkingRiskPoint, heatPoint));
                 place = movablePlaces[i];
                 riskPoint = checkingRiskPoint;
             }
         }

         return place;*/
        scoreSet = null;

        return null;
    }

}
