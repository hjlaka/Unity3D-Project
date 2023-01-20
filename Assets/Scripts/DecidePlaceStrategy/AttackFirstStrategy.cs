using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFirstStrategy : DecidePlaceStrategy, IDecidePlaceStrategy
{

    protected override void CopyData()
    {
        willingToDefend = StrategyManager.Instance.attackFirst.willingToDefend;
        willingToAttack = StrategyManager.Instance.attackFirst.willingToAttack;
        willingToThreat = StrategyManager.Instance.attackFirst.willingToThreat;
        willingToExtend = StrategyManager.Instance.attackFirst.willingToExtend;
        willingToSafe = StrategyManager.Instance.attackFirst.willingToSafe;
        futureOriented = StrategyManager.Instance.attackFirst.futureOriented;

    }
    public Place DecidePlace(Piece piece, ref float will, out ScoreNode scoreSet)
    {
        float maxScore = -99f;
        Place highScorePlace = null;
        string debug_score = "";
        scoreSet = null;

        List<Place> movablePlaces = piece.Recognized.movable;

        CopyData();
     

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            Place place = movablePlaces[i];
            ScoreNode tempScoreSet = CalculateScore(piece, place);
            float score = scoreSet.WillPoint;

            // 저장해 둔 점수 중 가장 높은 점수 가져오기? (왕 보호에 대한 것 외에는 차선을 가져올 수도 있도록?)
            // 점수 저장이 의미 있을까?
            if(score >= maxScore)
            {
                highScorePlace = place;
                maxScore = score;
                scoreSet = tempScoreSet;
            }
        }
        if(GameManager.Instance.scoreDebugMode)
        {
            Debug.Log(debug_score);
            Debug.Log(highScorePlace?.boardIndex + "가 최대 점수다. : " + maxScore);
        }


        will = maxScore / GetTotalWeight();

        return highScorePlace;
    }
   /* public Place DecidePlace2(Piece piece)
    {




        Place place = null;
        int heatPoint = 99;
        int attackPoint = -99;

        // 데이터에는 결함이 없는 상태로 가정한다.
        List<Place> movablePlaces = piece.MovableTo;
        List<Piece> attackablePieces = piece.ThreatTo;

        Debug.Log("공격 우선 선택");


        *//*
        
            과열도를 따져 기물이 이동할 곳을 결정하는 방식을 사용 중이다.
            그러나 현재 과열도는 기물이 이동할 때, 간접적인 변화가 업데이트 되고 있지 않다.

            과열도를 통한 결정 방식도 완전하지 못한 상태다.

        *//*



        // 공격 가능 확인
        if (attackablePieces.Count > 0)
        {

            for (int i = 0; i < attackablePieces.Count; i++)
            {

                // 적군 과열도보다 팀 과열도가 높은 곳 우선 선택.
                Place checkingPlace = attackablePieces[i].place;
                int teamHeat = piece.returnHeat.ReturnTeamHeat(checkingPlace);
                int oppenentHeat = piece.returnHeat.ReturnOpponentHeat(checkingPlace);

                int checkingAttackPoint = teamHeat - oppenentHeat;

                if (attackPoint <= checkingAttackPoint)
                {
                    // 과열도가 낮은 곳 우선 (선택 사항)
                    if (heatPoint < checkingPlace.HeatPoint) continue;

                    place = attackablePieces[i].place;
                    heatPoint = attackablePieces[i].place.HeatPoint;
                    attackPoint = checkingAttackPoint;
                }
            }

            return place;
        }



        // 공격이 불가능한 경우 (회피 우선)

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            Place checkingPlace = movablePlaces[i];
            //Debug.Log("선택된 기물의 팀 : " + piece.returnHeat);
            int teamHeat = piece.returnHeat.ReturnTeamHeat(checkingPlace);
            int oppenentHeat = piece.returnHeat.ReturnOpponentHeat(checkingPlace);

            // 위험도를 따져서 덜 위험한 곳으로 간다.
            int checkingRiskPoint = oppenentHeat - teamHeat;
            //Debug.Log(string.Format("{2} 자리 : 적 과열도 {0}, 아군 과열도 {1}", oppenentHeat, teamHeat, movablePlaces[i].boardIndex));

            //Debug.Log(string.Format("{2} 자리 : 기존 위험도 {0}, 이곳의 위험도 {1}", heatPoint, checkingRiskPoint, movablePlaces[i].boardIndex));
            if (checkingRiskPoint <= heatPoint)
            {
                //Debug.Log(string.Format("{0} 자리가 위험도가 {1} 이므로, {2} 였던 자리보다 안전하겠다.", movablePlaces[i].boardIndex, checkingRiskPoint, heatPoint));
                place = movablePlaces[i];
                heatPoint = checkingRiskPoint;
            }
        }

        return place;
    }*/

}
