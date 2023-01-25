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

            // ������ �� ���� �� ���� ���� ���� ��������? (�� ��ȣ�� ���� �� �ܿ��� ������ ������ ���� �ֵ���?)
            // ���� ������ �ǹ� ������?
            if (score >= maxScore)
            {
                highScorePlace = place;
                maxScore = score;
                highScoreSet = tempScoreSet;
            }
        }
        if (GameManager.Instance.scoreDebugMode)
        {
            Debug.Log(highScorePlace?.boardIndex + "�� �ִ� ������. : " + maxScore);
        }


        will = maxScore / GetTotalWeight();
        scoreSet = highScoreSet;

        Placement placement = new Placement(piece, piece.place, highScorePlace, highScorePlace?.Piece, null);

        return placement;
    }
}
