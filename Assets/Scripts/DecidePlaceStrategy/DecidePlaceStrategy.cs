using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecidePlaceStrategy : IDecidePlaceStrategy
{
    protected ScoreMap scoreMap = new ScoreMap();

    protected IHeatPreperStrategy heatPreperStrategy;

    protected StrategyData strategyData;


    protected float willingToAttack = 1.1f;
    protected float willingToDefend = 1.1f;
    protected float willingToExtend = 0.5f;
    protected float willingToSafe = 1.1f;
    protected float willingToThreat = 1f;
    protected float futureOriented = 0.1f;
    protected float floatingMind = 0.1f;

    protected float totalWeight;

    protected virtual void CopyData()
    {
        if(strategyData == null) return;

        willingToDefend = strategyData.willingToDefend;
        willingToAttack = strategyData.willingToAttack;
        willingToThreat = strategyData.willingToThreat;
        willingToExtend = strategyData.willingToExtend;
        willingToSafe = strategyData.willingToSafe;
        futureOriented = strategyData.futureOriented;
        floatingMind = strategyData.floatingMind;
    }

    protected float GetTotalWeight()
    {
        totalWeight = willingToAttack + willingToDefend + willingToExtend + willingToSafe + willingToThreat + futureOriented;

        return totalWeight;
    }
    public virtual Placement DecidePlace(Piece piece, ref float will, out ScoreNode scoreSet)
    {
        float maxScore = -99f;
        Place highScorePlace = null;
        ScoreNode highScoreSet = null;

        List<Place> movablePlaces = piece.Recognized.movable;

        Debug.Log("�� �� �ִ� �ڸ� ��: " + movablePlaces.Count);

        if (movablePlaces.Count <= 0)
        {
            scoreSet = null;
            return null;
        }

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            Place place = movablePlaces[i];
            ScoreNode tempScoreSet = CalculateScore(piece, place);
            if (GameManager.Instance.scoreDebugMode) Debug.Log("���� ����: " + tempScoreSet.WillPoint);
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

        if(GameManager.Instance.scoreDebugMode) Debug.Log(string.Format("���� ������ ���� �ڸ� {0} ���� ���� {1}", highScorePlace, will));
        Placement placement = new Placement(piece, piece.place, highScorePlace, highScorePlace?.Piece, null);

        return placement;
    }

    protected virtual ScoreNode CalculateScore(Piece piece, Place place)
    {
        float score;
        float attackPoint;
        float placePreferPoint;
        float defencePoint = 0;
        float assumePoint;
        float mindPoint;
        float extentPoint;
        float heatPreferPoint;
        int deltaHeat;
        //float futureOrientPoint;

        Piece targetPiece = place.Piece;

        attackPoint = CalculateAttackPoint(targetPiece);
        attackPoint *= willingToAttack;

        deltaHeat = piece.returnHeat.ReturnTeamHeat(place) - piece.returnHeat.ReturnOpponentHeat(place) - 1;  // �ڱ� �ڽ��� ������ �߱�
        heatPreferPoint = CalculateHeatPreferPoint(piece, place, deltaHeat);

        //������ ����
        // ������ ���� = (������ ��ȣ ���� / (1 + ������ ��ȣ ����))
        // 0, 1/2, 2/3, ...
        float tempPieceScore = 1 - ((float)piece.PieceScore / (1 + piece.PieceScore));
        tempPieceScore *= 3;
        heatPreferPoint += tempPieceScore;
        placePreferPoint = heatPreferPoint / (1 + heatPreferPoint);

        extentPoint = CalculateExtentPoint(piece, place);
        extentPoint *= willingToExtend;

        // �̵��غ������� ����ؾ��Ѵ�.
        assumePoint = CalculateAssumeMoveScore(piece, place);
        assumePoint *= futureOriented;
        //assumePoint = 0;
        if (GameManager.Instance.scoreDebugMode) Debug.Log("���� ����: " + assumePoint);

        mindPoint = Random.Range(0, 3);
        mindPoint = mindPoint / (1 + mindPoint);
        mindPoint *= floatingMind;

        score = attackPoint + placePreferPoint + assumePoint + extentPoint + mindPoint;
        if (GameManager.Instance.scoreDebugMode)
            Debug.Log(place.boardIndex + " == ���� ����: " + attackPoint + " / �̵� ����: " + placePreferPoint + " /  ���� ����: " + assumePoint + 
                " / Ȯ�� ����: " + extentPoint + "/ ���� ����: " + mindPoint);

        ScoreNode scoreSet = new ScoreNode();
        scoreSet.AttackPoint = attackPoint;
        scoreSet.DefencdPoint = defencePoint;
        scoreSet.ExtendPoint = assumePoint;
        scoreSet.DeltaHeatPoint = deltaHeat;
        scoreSet.DefencdPoint = defencePoint;
        scoreSet.WillPoint = score;

        StoreToMap(place.boardIndex, scoreSet);


        return scoreSet;
    }

    protected void StoreToMap(Vector2Int location, ScoreNode scoreSet)
    {
        scoreMap.SetNode(location, scoreSet);
    }

    protected float CalculateAssumeMoveScore(Piece piece, Place place)
    {
        float assumScore;
        float defendablePoint = 0;
        float threatablePoint = 0;
        float extendablePoint = 0;
        float heatPreferPoint = 0;


        StateLists assumeStateLists = new StateLists();

        Vector2Int location = place.boardIndex;
        
        // �ѹ� ������ ��Ȳ���� �����Ѵ�.
        piece.MoveCount++;


        // �⹰�� �������ٰ� ������ ��, �⹰�� �����Ӵ�� ����� ��ġ�� ����Ѵ�.
        // �⹰�� ���� ���� ������ ������ ��ġ�� �ʴ°�?
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
                maxHeatPoint = deltaHeatPoint;  // Ȯ�� ������ ���ʽ�
        }

        // �̵� ���� Ȯ�� 
        // Ȯ�� ���� = �� �� �ִ� ��ġ ���� / (1 + �⹰ ����)
        extendablePoint = (float)movableCount / (piece.PieceScore * piece.PieceScore);

        extendablePoint *= willingToExtend;
        if (GameManager.Instance.scoreDebugMode) Debug.Log("Ȯ�� ����: " + extendablePoint);

        if (piece.returnHeat.ReturnOpponentHeat(place) > 0)
        {
            heatPreferPoint *= 0.5f;
        }

        // ������ �� �ִ� �⹰
        for (int i = 0; i < assumeStateLists.threating.Count; i++)
        {
            // ���� ���� �Ŀ� ���� ���� ������ ��ȯ�޴´�.
            Piece threatablePiece = assumeStateLists.threating[i];

            if (threatablePiece.PieceScore < piece.PieceScore) continue;

            // ���� ���� = ������ �� �ִ� �⹰�鿡 ���� ���� ������ ��
            threatablePoint += (float)(threatablePiece.PieceScore) / (1 + threatablePiece.PieceScore);
        }

        // ������ �ڽ��� ���迡 ����Ǵ� ��Ȳ�̱⵵ �ϴ�. - ������ ���� �̵��� ���� ���̹Ƿ� ���� ����� �ʿ� ����.
        threatablePoint *= willingToThreat;

        // ����� �� �ִ� �⹰
        for (int i = 0; i < assumeStateLists.defending.Count; i++)
        {
            Piece defendablePiece = assumeStateLists.defending[i];
            Debug.Log("��� ��� �⹰: " + defendablePiece);
            // ������ ��Ȳ���� �ڽ��� ����ϴ� ��츦 ���� �Ѵ�. - �⹰�� �ӽ� �̵� ���״�.

            int targetDeltaHeat = piece.returnHeat.ReturnTeamHeat(defendablePiece.place) - piece.returnHeat.ReturnOpponentHeat(defendablePiece.place);
            int backUpPoint;
            if (targetDeltaHeat < 0) backUpPoint = 2;
            if (targetDeltaHeat == 0) backUpPoint = 1;
            else backUpPoint = 0;

            defendablePoint += ((float)(defendablePiece.PieceScore) / (1 + defendablePiece.PieceScore)) * backUpPoint; // * �������� ����� ����. (������ ���� ���� ���� �����?)

            // ������ �ʿ��� ���� + �����ϰ��� �ϴ� ���� (�ջ�? ��?)
        }

        defendablePoint *= willingToDefend;

        assumScore = /*extendablePoint +*/ heatPreferPoint + threatablePoint + defendablePoint;
        if(GameManager.Instance.scoreDebugMode)
            Debug.Log(string.Format("{0}�������� ������ ��ġ ���� {5}-------\n �����: {1}, ������ ������ ����: {2}, ����: {3}, ��ȣ {4}",
            location, extendablePoint, heatPreferPoint, threatablePoint, defendablePoint, assumScore));


        // �ǵ��� ����
        piece.place.Piece = piece;
        piece.MoveCount--;
        return assumScore;


    }

    private float CalculateAttackPoint(Piece targetPiece)
    {
        float attackPoint = 0;

        if (targetPiece != null)         // ���� ������ ��Ҷ��
        {
            // �⹰�� ������ Ȯ���Ѵ�.
            // �⹰�� ���� ���Ǹ� Ȯ���Ѵ�. �����ɸ�ŭ ���Ѵ�.

            // �⹰�� ������ �Ŀ� ���� �Ǵ��Ѵ�.

            attackPoint = (float)targetPiece.PieceScore / (1 + targetPiece.PieceScore);

        }
        if (GameManager.Instance.scoreDebugMode) Debug.Log("��������: " + attackPoint);

        return attackPoint;
    }

    private float CalculateHeatPreferPoint(Piece piece, Place place, int deltaHeat)
    {
        // ������ ����?
        // �������� ���� ���� �� ���ΰ�?
        // �������� ���� ���� �� ���ΰ�?

        //���� �켱:

        // ����� ������ �ϳ��� ������ �켱���� �������� ���
        // ����� ������ �־, �߿䵵�� ���ٸ�(?), ���� �ϰ� �̵��ϴ� ���

        // ���� �� �ִ� �⹰�� ��쿡��... �� �⹰�� �������� �ϳ� ���� �Ѵ�.
        
        float heatPreferPoint = 0;
        if (0 == deltaHeat)
        {
            heatPreferPoint = 3;
        }
        else if (deltaHeat == 1)
        {
            heatPreferPoint = 5;
        }
        else if (deltaHeat == -1)
        {
            heatPreferPoint = 1;
        }
        else if (deltaHeat > 1)
        {
            heatPreferPoint = 1;
        }
        else if (deltaHeat < -1)
        {
            heatPreferPoint = 0;
        }

        if (piece.returnHeat.ReturnOpponentHeat(place) > 0)
        {
            heatPreferPoint *= 0.5f;
        }

        return heatPreferPoint;
    }

    private float CalculateExtentPoint(Piece piece, Place place)
    {
        float extentPoint = Mathf.Abs(place.boardIndex.x - piece.place.boardIndex.x) + Mathf.Abs(place.boardIndex.y - piece.place.boardIndex.y);
        extentPoint /= piece.PieceScore;
        extentPoint = extentPoint / (1 + extentPoint);
        if (GameManager.Instance.scoreDebugMode)
            Debug.Log("�ָ� ���� ����: " + extentPoint);

        return extentPoint;
    }
}
