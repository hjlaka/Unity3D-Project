using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStrategy : DecidePlaceStrategy
{
    public override Placement DecidePlace(Piece piece, ref float will, out ScoreNode scoreSet)
    {
        float maxScore = -99f;
        Place highScorePlace = null;
        ScoreNode highScoreSet = null;

        List<Place> movablePlaces = piece.Recognized.movable;

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            Place place = movablePlaces[i];
            ScoreNode tempScoreSet = CalculateScore(piece, place);
            float score = tempScoreSet.WillPoint;

            // 저장해 둔 점수 중 가장 높은 점수 가져오기? (왕 보호에 대한 것 외에는 차선을 가져올 수도 있도록?)
            // 점수 저장이 의미 있을까?
            if (score >= maxScore)
            {
                highScorePlace = place;
                maxScore = score;
                highScoreSet = tempScoreSet;
            }
        }
        if (GameManager.Instance.scoreDebugMode)
        {
            Debug.Log(highScorePlace?.boardIndex + "가 최대 점수다. : " + maxScore);
        }


        will = maxScore / GetTotalWeight();
        scoreSet = highScoreSet;

        Placement placement = new Placement(piece, piece.place, highScorePlace, highScorePlace?.Piece, null);

        return placement;
    }
}
