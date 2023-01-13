using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecidePlaceStrategy : MonoBehaviour
{
    protected int[,] scores;

    IHeatPreperStrategy heatPreperStrategy;

    public DecidePlaceStrategy()
    {
        scores = new int[8, 8];
    }

    protected void ClearScores()
    {
        for (int i = 0; i < scores.GetLength(0); i++)
        {
            for (int j = 0; j < scores.GetLength(1); j++)
            {
                scores[i, j] = -99;
            }
        }
    }
    protected virtual int CalculateScore(Piece piece, Place place)
    {
        int score;
        int attackPoint = 0;
        int heatPreferPoint = 0;
        Piece targetPiece = place.piece;

        if (targetPiece != null)         // 공격 가능한 장소라면
        {
            // 기물의 점수를 확인한다.
            // 기물에 대한 적의를 확인한다. 적개심만큼 곱한다.

            // 기물을 공격한 후에 대해 판단한다.
            // 안전 점수는 언제 계산할까?
            attackPoint += targetPiece.PieceScore;
        }
        // 과열도 점수?
        // 과열도가 높은 곳에 갈 것인가?
        // 과열도가 낮은 곳에 갈 것인가?

        //안정 우선:
        heatPreferPoint = piece.returnHeat.ReturnTeamHeat(place) - piece.returnHeat.ReturnOpponentHeat(place);

        // 이동해본다음에 계산해야한다.
        // 공격 점수 계산

        // 이동 점수 계산

        // 방어 점수 계산

        score = heatPreferPoint;
        return score;
    }


    protected void AddScoreToLocation(Vector2Int location, int score)
    {
        scores[location.x, location.y] = score;
    }
}
