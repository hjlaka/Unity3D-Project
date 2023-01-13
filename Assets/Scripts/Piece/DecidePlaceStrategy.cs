using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecidePlaceStrategy : MonoBehaviour
{
    protected int[,] scores;

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
    protected virtual int CalculateScore(Place place)
    {
        int score;
        int attackPoint = 0;
        Piece piece = place.piece;

        if (piece != null)         // ���� ������ ��Ҷ��
        {
            // �⹰�� ������ Ȯ���Ѵ�.
            // �⹰�� ���� ���Ǹ� Ȯ���Ѵ�. �����ɸ�ŭ ���Ѵ�.

            // �⹰�� ������ �Ŀ� ���� �Ǵ��Ѵ�.
            // ���� ������ ���� ����ұ�?
            attackPoint += piece.PieceScore;
        }
        // ������ ����?
        // �̵��غ������� ����ؾ��Ѵ�.
        // ���� ���� ���

        // �̵� ���� ���

        // ��� ���� ���

        score = attackPoint;
        return score;
    }


    protected void AddScoreToLocation(Vector2Int location, int score)
    {
        scores[location.x, location.y] = score;
    }
}
