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

        if (targetPiece != null)         // ���� ������ ��Ҷ��
        {
            // �⹰�� ������ Ȯ���Ѵ�.
            // �⹰�� ���� ���Ǹ� Ȯ���Ѵ�. �����ɸ�ŭ ���Ѵ�.

            // �⹰�� ������ �Ŀ� ���� �Ǵ��Ѵ�.
            // ���� ������ ���� ����ұ�?
            attackPoint += targetPiece.PieceScore;
        }
        // ������ ����?
        // �������� ���� ���� �� ���ΰ�?
        // �������� ���� ���� �� ���ΰ�?

        //���� �켱:
        heatPreferPoint = piece.returnHeat.ReturnTeamHeat(place) - piece.returnHeat.ReturnOpponentHeat(place);

        // �̵��غ������� ����ؾ��Ѵ�.
        // ���� ���� ���

        // �̵� ���� ���

        // ��� ���� ���

        score = heatPreferPoint;
        return score;
    }


    protected void AddScoreToLocation(Vector2Int location, int score)
    {
        scores[location.x, location.y] = score;
    }
}
