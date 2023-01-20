using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendFirstStategy : DecidePlaceStrategy, IDecidePlaceStrategy
{
    protected override void CopyData()
    {
        willingToDefend = StrategyManager.Instance.defendFirst.willingToDefend;
        willingToAttack = StrategyManager.Instance.defendFirst.willingToAttack;
        willingToThreat = StrategyManager.Instance.defendFirst.willingToThreat;
        willingToExtend = StrategyManager.Instance.defendFirst.willingToExtend;
        willingToSafe = StrategyManager.Instance.defendFirst.willingToSafe;
        futureOriented = StrategyManager.Instance.defendFirst.futureOriented;
    }
    public Place DecidePlace(Piece piece, ref float will)
    {
        float maxScore = -99f;
        Place highScorePlace = null;
        ScoreNode highWillScoreSet = null;
        string debug_score = "";

        List<Place> movablePlaces = piece.Recognized.movable;

        CopyData();
        scoreMap.ClearMap();

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            Place place = movablePlaces[i];
            Vector2Int location = movablePlaces[i].boardIndex;
            ScoreNode scoreSet = CalculateScore(piece, place);
            float score = scoreSet.WillPoint;
            debug_score += score + " / ";

            // ������ �� ���� �� ���� ���� ���� ��������? (�� ��ȣ�� ���� �� �ܿ��� ������ ������ ���� �ֵ���?)
            // ���� ������ �ǹ� ������?
            if (score >= maxScore)
            {
                highScorePlace = place;
                maxScore = score;
                highWillScoreSet = scoreSet;
            }
        }
        if (GameManager.Instance.scoreDebugMode)
        {
            Debug.Log(debug_score);
            Debug.Log(highScorePlace.boardIndex + "�� �ִ� ������. : " + maxScore);
        }

        scoreMap.PrintMap();

        will = maxScore / GetTotalWeight();

        return highScorePlace;
    }
    /* public Place DecidePlace(Piece piece)
     {
         Debug.Log("��� ���� ����");

         Place place = null;


         // �ϴ� �̵��غ��� �� �������� �� �������� ������ �Ѵ�.

         // Ȥ�� �� ������ ä �̵��ϱ�?

         // Ȥ�� �׳� ȸ�� �̵� �켱?


         *//*int defencePoint = 0;

         for (int i = 0; i < movablePlaces.Count; i++)
         {
             if(movablePlaces[i].HeatPoint >= defencePoint)
             {
                 place = movablePlaces[i];
                 defencePoint = movablePlaces[i].HeatPoint;
             }    
         }
 *//*
         return place;

     }*/
}
