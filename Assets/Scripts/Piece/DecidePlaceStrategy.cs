using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecidePlaceStrategy
{
    protected int[,] scores;

    IHeatPreperStrategy heatPreperStrategy;

    StrategyData strategyData;


    protected float willingToDefend = 1f;
    protected float willingToAttack = 4f;
    protected float willingToThreat = 1.2f;
    protected float willingToExtend = 0.5f;
    protected float willingToSafe = 1.3f;
    protected float futureOriented = 0.1f;

    public DecidePlaceStrategy()
    {
        scores = new int[8, 8];
    }

    protected virtual void CopyData()
    {

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
        float placePreferPoint;
        float defencePoint = 0;
        float assumePoint;
        //float futureOrientPoint;

        Piece targetPiece = place.Piece;

        if (targetPiece != null)         // ���� ������ ��Ҷ��
        {
            // �⹰�� ������ Ȯ���Ѵ�.
            // �⹰�� ���� ���Ǹ� Ȯ���Ѵ�. �����ɸ�ŭ ���Ѵ�.

            // �⹰�� ������ �Ŀ� ���� �Ǵ��Ѵ�.
            // ���� ������ ���� ����ұ�?
            //attackPoint += targetPiece.PieceScore;

            //���� ���� = (�⹰ ���� / (1 + �⹰ ����))
            // 1/2, 3/4, 5/6, 9/10
            // ���� ����?
            attackPoint = (float)targetPiece.PieceScore / (1 + targetPiece.PieceScore);

            attackPoint *= willingToAttack;


        }
        // ������ ����?
        // �������� ���� ���� �� ���ΰ�?
        // �������� ���� ���� �� ���ΰ�?

        //���� �켱:

        // ����� ������ �ϳ��� ������ �켱���� �������� ���
        // ����� ������ �־, �߿䵵�� ���ٸ�(?), ���� �ϰ� �̵��ϴ� ���

        // ���� �� �ִ� �⹰�� ��쿡��... �� �⹰�� �������� �ϳ� ���� �Ѵ�.
        int deltaHeat = piece.returnHeat.ReturnTeamHeat(place) - piece.returnHeat.ReturnOpponentHeat(place) - 1;  // �ڱ� �ڽ��� ������ �߱�
        float heatPreferPoint = 0;
        if (0 == deltaHeat)
        {
            heatPreferPoint = 3;
        }
        else if (deltaHeat == 1)
        {
            heatPreferPoint = 4;
        }
        else if (deltaHeat == -1)
        {
            heatPreferPoint = 1;
        }
        else if (deltaHeat > 1)
        {
            heatPreferPoint = 4;
        }
        else if (deltaHeat < -1)
        {
            heatPreferPoint = 0;
        }

        if(piece.returnHeat.ReturnOpponentHeat(place) > 0)
        {
            heatPreferPoint *= 0.5f;
        }

        //������ ����
        // ������ ���� = (������ ��ȣ ���� / (1 + ������ ��ȣ ����))
        // 0, 1/2, 2/3, ...
        placePreferPoint = heatPreferPoint / (1 + heatPreferPoint);



        // �̵��غ������� ����ؾ��Ѵ�.
        assumePoint = CalculateAssumeMoveScore(piece, place);
        assumePoint *= futureOriented;
        //assumePoint = 0;


        score = attackPoint + placePreferPoint + assumePoint;
        if (GameManager.Instance.scoreDebugMode)
            Debug.Log(place.boardIndex + " == ���� ����: " + attackPoint + " / �̵� ����: " + placePreferPoint + " /  ���� ����: " + assumePoint);
        return score;
    }

    protected float CalculateAssumeMoveScore(Piece piece, Place place)
    {
        float assumScore;
        float defendablePoint = 0;
        float threatablePoint = 0;
        float extendablePoint = 0;
        float heatPreferPoint = -99;


        StateLists assumeStateLists = new StateLists();

        Vector2Int location = place.boardIndex;



        // �⹰�� �������ٰ� ������ ��, �⹰�� �����Ӵ�� ������� ����Ѵ�.
        piece.place.Piece = null;
        piece.MovePattern.RecognizeRange(location, assumeStateLists);

        // �̵��� �� �ִ� ĭ ��
        // + �̵��� �� �ִ� ĭ �� �ִ� ������?
        // Ȥ�� �� �������� ������ ��? - ��� �켱
        // Ȥ�� �� �������� �� ���� ��? - ���� �켱
        int movableCount = 0;
        int maxHeatPoint = -99;

        for (int i = 0; i < assumeStateLists.movable.Count; i++)
        {
            Place assumePlace = assumeStateLists.movable[i];

            // ��ġ ��ȣ���� �Բ� ����Ѵ�. - ��ġ ��ȣ ������ ������.
            int deltaHeatPoint = piece.returnHeat.ReturnTeamHeat(assumePlace) - piece.returnHeat.ReturnOpponentHeat(assumePlace);
            // ����: �ڽ��� �������� �Բ� ����ǰ� �ִ�.

            movableCount += 1;

            if (maxHeatPoint < deltaHeatPoint)
                maxHeatPoint = deltaHeatPoint;
        }

        // �̵� ���� Ȯ�� 
        // Ȯ�� ���� = �� �� �ִ� ��ġ ���� / (1 + ��ġ ����)
        extendablePoint = (float)movableCount / (1 + movableCount);

        extendablePoint *= willingToExtend;

        heatPreferPoint = (float)(heatPreferPoint) / (1 + heatPreferPoint);
        heatPreferPoint *= willingToSafe;

        // ������ �� �ִ� �⹰
        for (int i = 0; i < assumeStateLists.threating.Count; i++)
        {
            // ���� ���� �Ŀ� ���� ���� ������ ��ȯ�޴´�.
            Piece threatablePiece = assumeStateLists.threating[i];

            // ���� ���� = ������ �� �ִ� �⹰�鿡 ���� ���� ������ ��
            threatablePoint += (float)(threatablePiece.PieceScore) / (1 + threatablePiece.PieceScore);
        }

        // ������ �ڽ��� ���迡 ����Ǵ� ��Ȳ�̱⵵ �ϴ�.
        threatablePoint *= (willingToThreat - willingToSafe);

        // ����� �� �ִ� �⹰
        for (int i = 0; i < assumeStateLists.defending.Count; i++)
        {
            Piece defendablePiece = assumeStateLists.defending[i];
            Debug.Log("��� ��� �⹰: " + defendablePiece);
            // ������ ��Ȳ���� �ڽ��� ����ϴ� ��츦 ���� �Ѵ�. - �⹰�� �ӽ� �̵� ���״�.

            defendablePoint += (float)(defendablePiece.PieceScore) / (1 + defendablePiece.PieceScore); // * �������� ����� ����. (������ ���� ���� ���� �����?)

            // ������ �ʿ��� ���� + �����ϰ��� �ϴ� ���� (�ջ�? ��?)
        }

        defendablePoint *= willingToDefend;

        assumScore = extendablePoint + heatPreferPoint + threatablePoint + defendablePoint;
        if(GameManager.Instance.scoreDebugMode)
            Debug.Log(string.Format("{0}�������� ������ ��ġ ���� {5}-------\n �����: {1}, ������ ������ ����: {2}, ����: {3}, ��ȣ {4}",
            location, extendablePoint, heatPreferPoint, threatablePoint, defendablePoint, assumScore));


        // �ǵ��� ����
        piece.place.Piece = piece;
        return assumScore;


    }


    protected void AddScoreToLocation(Vector2Int location, int score)
    {
        scores[location.x, location.y] = score;
    }

}
