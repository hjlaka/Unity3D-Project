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

            // 저장해 둔 점수 중 가장 높은 점수 가져오기? (왕 보호에 대한 것 외에는 차선을 가져올 수도 있도록?)
            // 점수 저장이 의미 있을까?
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
            Debug.Log(highScorePlace.boardIndex + "가 최대 점수다. : " + maxScore);
        }

        scoreMap.PrintMap();

        will = maxScore / GetTotalWeight();

        Placement placement = new Placement(piece, piece.place, highScorePlace, highScorePlace.Piece, null);

        return placement;
    }
    /* public Place DecidePlace(Piece piece)
     {
         Debug.Log("방어 먼저 전략");

         Place place = null;


         // 일단 이동해보고 방어가 가능한지 안 가능한지 따져야 한다.

         // 혹은 방어를 유지한 채 이동하기?

         // 혹은 그냥 회피 이동 우선?


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
