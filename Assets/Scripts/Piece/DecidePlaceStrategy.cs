using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecidePlaceStrategy
{
    protected int[,] scores;

    IHeatPreperStrategy heatPreperStrategy;


    private float willingToDefend = 1f;
    private float willingToAttack = 4f;
    private float willingToThreat = 1.2f;
    private float willingToExtend = 0.5f;
    private float willingToSafe = 1.3f;
    private float futureOriented = 0.1f;

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
    protected virtual float CalculateScore(Piece piece, Place place)
    {
        float score;
        float attackPoint = 0;
        float heatPreferPoint;
        float defencePoint = 0;
        float assumePoint;

        Piece targetPiece = place.piece;

        if (targetPiece != null)         // ���� ������ ��Ҷ��
        {
            // �⹰�� ������ Ȯ���Ѵ�.
            // �⹰�� ���� ���Ǹ� Ȯ���Ѵ�. �����ɸ�ŭ ���Ѵ�.

            // �⹰�� ������ �Ŀ� ���� �Ǵ��Ѵ�.
            // ���� ������ ���� ����ұ�?
            attackPoint += targetPiece.PieceScore;

            attackPoint *= willingToAttack;
        }
        // ������ ����?
        // �������� ���� ���� �� ���ΰ�?
        // �������� ���� ���� �� ���ΰ�?

        //���� �켱:
        heatPreferPoint = piece.returnHeat.ReturnTeamHeat(place) - piece.returnHeat.ReturnOpponentHeat(place) - 1;  // �ڱ� �ڽ��� ������ �߱�

        // �̵��غ������� ����ؾ��Ѵ�.
        assumePoint = CalculateAssumeMoveScore(piece, place);
        assumePoint *= futureOriented;
        
        score = attackPoint + heatPreferPoint + assumePoint;
        Debug.Log(place.boardIndex + " == ���� ����: " + attackPoint + " / �̵� ����: " + heatPreferPoint + " /  ���� ����: " + assumePoint);
        return score;
    }

    protected float CalculateAssumeMoveScore(Piece piece, Place place)
    {
        float assumScore;
        float defendablePoint = 0;
        float threatablePoint = 0;
        float influencablePoint = 0;
        float heatPreferPoint = -99;


        StateLists assumeStateLists = new StateLists();

        Vector2Int location = place.boardIndex;



        // �⹰�� �������ٰ� ������ ��, �⹰�� �����Ӵ�� ������� ����Ѵ�.
        piece.place.piece = null;
        piece.MovePattern.RecognizeRange(location, assumeStateLists);

        // �̵��� �� �ִ� ĭ ��
        // + �̵��� �� �ִ� ĭ �� �ִ� ������?
        // Ȥ�� �� �������� ������ ��? - ��� �켱
        // Ȥ�� �� �������� �� ���� ��? - ���� �켱
        for (int i = 0; i < assumeStateLists.movable.Count; i++)
        {
            Place assumePlace = assumeStateLists.movable[i];
            float deltaHeatPoint = piece.returnHeat.ReturnTeamHeat(assumePlace) - piece.returnHeat.ReturnOpponentHeat(assumePlace);
            // ����: �ڽ��� �������� �Բ� ����ǰ� �ִ�.

            influencablePoint += 1;

            if (heatPreferPoint < deltaHeatPoint)
                heatPreferPoint = deltaHeatPoint;
        }

        influencablePoint *= willingToExtend;

        heatPreferPoint *= willingToSafe;

        // ������ �� �ִ� �⹰
        for (int i = 0; i < assumeStateLists.threating.Count; i++)
        {
            Piece threatablePiece = assumeStateLists.threating[i];
            threatablePoint += threatablePiece.PieceScore;
        }

        threatablePoint *= (willingToThreat - willingToSafe);

        // ����� �� �ִ� �⹰
        for (int i = 0; i < assumeStateLists.defending.Count; i++)
        {
            Piece defendablePiece = assumeStateLists.defending[i];
            Debug.Log("��� ��� �⹰: " + defendablePiece);
            // ������ ��Ȳ���� �ڽ��� ����ϴ� ��츦 ���� �Ѵ�.
            defendablePoint += defendablePiece.PieceScore; // + �������� ����� ����. (������ ���� ���� ���� �����?)
        }

        defendablePoint *= willingToDefend;

        assumScore = influencablePoint + heatPreferPoint + threatablePoint + defendablePoint;
        Debug.Log(string.Format("{0}�������� ������ ��ġ ���� {5}-------\n �����: {1}, ������ ������ ����: {2}, ����: {3}, ��ȣ {4}",
            location, influencablePoint, heatPreferPoint, threatablePoint, defendablePoint, assumScore));


        // �ǵ��� ����
        piece.place.piece = piece;
        return assumScore;


    }


    protected void AddScoreToLocation(Vector2Int location, int score)
    {
        scores[location.x, location.y] = score;
    }

}
