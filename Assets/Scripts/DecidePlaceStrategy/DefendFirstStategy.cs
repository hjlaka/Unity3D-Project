using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendFirstStategy : DecidePlaceStrategy
{
    public DefendFirstStategy()
    {
        strategyData = StrategyManager.Instance.defendFirst;
    }
    public override Placement DecidePlace(Piece piece, ref float will, out ScoreNode scoreSet)
    {
        float maxScore = -99f;
        Place highScorePlace = null;
        string debug_score = "";
        scoreSet = null;

        List<Place> movablePlaces = piece.Recognized.movable;

        CopyData();
        scoreMap.ClearMap();

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            Place place = movablePlaces[i];
            Vector2Int location = movablePlaces[i].boardIndex;
            ScoreNode tempScoreSet = CalculateScore(piece, place);
            float score = scoreSet.WillPoint;
            debug_score += score + " / ";

            // ������ �� ���� �� ���� ���� ���� ��������? (�� ��ȣ�� ���� �� �ܿ��� ������ ������ ���� �ֵ���?)
            // ���� ������ �ǹ� ������?
            if (score >= maxScore)
            {
                highScorePlace = place;
                maxScore = score;
                scoreSet = tempScoreSet;
            }
        }
        if (GameManager.Instance.scoreDebugMode)
        {
            Debug.Log(debug_score);
            Debug.Log(highScorePlace.boardIndex + "�� �ִ� ������. : " + maxScore);
        }

        scoreMap.PrintMap();

        will = maxScore / GetTotalWeight();

        Placement placement = new Placement(piece, piece.place, highScorePlace, highScorePlace.Piece, null);

        return placement;
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
