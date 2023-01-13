using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFirstStrategy : DecidePlaceStrategy, IDecidePlaceStrategy
{

    public Place DecidePlace(Piece piece)
    {
        float maxScore = -99f;
        Place highScorePlace = null;
        string debug_score = "";

        List<Place> movablePlaces = piece.Recognized.movable;

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            Place place = movablePlaces[i];
            Vector2Int location = movablePlaces[i].boardIndex;
            float score = CalculateScore(piece, place);
            debug_score += score + " / ";

            // ������ �� ���� �� ���� ���� ���� ��������? (�� ��ȣ�� ���� �� �ܿ��� ������ ������ ���� �ֵ���?)
            // ���� ������ �ǹ� ������?
            if(score >= maxScore)
            {
                highScorePlace = place;
                maxScore = score;
            }
        }
        Debug.Log(debug_score);
        Debug.Log(highScorePlace.boardIndex + "�� �ִ� ������. : " + maxScore);

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
