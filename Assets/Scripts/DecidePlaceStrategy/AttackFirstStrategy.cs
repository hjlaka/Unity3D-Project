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

            // ������ �� ���� �� ���� ���� ���� ��������? (�� ��ȣ�� ���� �� �ܿ��� ������ ������ ���� �ֵ���?)
            // ���� ������ �ǹ� ������?
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
            Debug.Log(highScorePlace?.boardIndex + "�� �ִ� ������. : " + maxScore);
        }


        will = maxScore / GetTotalWeight();

        return highScorePlace;
    }
   /* public Place DecidePlace2(Piece piece)
    {




        Place place = null;
        int heatPoint = 99;
        int attackPoint = -99;

        // �����Ϳ��� ������ ���� ���·� �����Ѵ�.
        List<Place> movablePlaces = piece.MovableTo;
        List<Piece> attackablePieces = piece.ThreatTo;

        Debug.Log("���� �켱 ����");


        *//*
        
            �������� ���� �⹰�� �̵��� ���� �����ϴ� ����� ��� ���̴�.
            �׷��� ���� �������� �⹰�� �̵��� ��, �������� ��ȭ�� ������Ʈ �ǰ� ���� �ʴ�.

            �������� ���� ���� ��ĵ� �������� ���� ���´�.

        *//*



        // ���� ���� Ȯ��
        if (attackablePieces.Count > 0)
        {

            for (int i = 0; i < attackablePieces.Count; i++)
            {

                // ���� ���������� �� �������� ���� �� �켱 ����.
                Place checkingPlace = attackablePieces[i].place;
                int teamHeat = piece.returnHeat.ReturnTeamHeat(checkingPlace);
                int oppenentHeat = piece.returnHeat.ReturnOpponentHeat(checkingPlace);

                int checkingAttackPoint = teamHeat - oppenentHeat;

                if (attackPoint <= checkingAttackPoint)
                {
                    // �������� ���� �� �켱 (���� ����)
                    if (heatPoint < checkingPlace.HeatPoint) continue;

                    place = attackablePieces[i].place;
                    heatPoint = attackablePieces[i].place.HeatPoint;
                    attackPoint = checkingAttackPoint;
                }
            }

            return place;
        }



        // ������ �Ұ����� ��� (ȸ�� �켱)

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            Place checkingPlace = movablePlaces[i];
            //Debug.Log("���õ� �⹰�� �� : " + piece.returnHeat);
            int teamHeat = piece.returnHeat.ReturnTeamHeat(checkingPlace);
            int oppenentHeat = piece.returnHeat.ReturnOpponentHeat(checkingPlace);

            // ���赵�� ������ �� ������ ������ ����.
            int checkingRiskPoint = oppenentHeat - teamHeat;
            //Debug.Log(string.Format("{2} �ڸ� : �� ������ {0}, �Ʊ� ������ {1}", oppenentHeat, teamHeat, movablePlaces[i].boardIndex));

            //Debug.Log(string.Format("{2} �ڸ� : ���� ���赵 {0}, �̰��� ���赵 {1}", heatPoint, checkingRiskPoint, movablePlaces[i].boardIndex));
            if (checkingRiskPoint <= heatPoint)
            {
                //Debug.Log(string.Format("{0} �ڸ��� ���赵�� {1} �̹Ƿ�, {2} ���� �ڸ����� �����ϰڴ�.", movablePlaces[i].boardIndex, checkingRiskPoint, heatPoint));
                place = movablePlaces[i];
                heatPoint = checkingRiskPoint;
            }
        }

        return place;
    }*/

}
