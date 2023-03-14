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

        Debug.Log("갈 수 있는 자리 수: " + movablePlaces.Count);

        if (movablePlaces.Count <= 0)
        {
            scoreSet = null;
            return null;
        }

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            Place place = movablePlaces[i];
            ScoreNode tempScoreSet = CalculateScore(piece, place);
            if (GameManager.Instance.scoreDebugMode) Debug.Log("의지 점수: " + tempScoreSet.WillPoint);
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

        if(GameManager.Instance.scoreDebugMode) Debug.Log(string.Format("가장 의지가 높은 자리 {0} 의지 점수 {1}", highScorePlace, will));
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

        deltaHeat = piece.returnHeat.ReturnTeamHeat(place) - piece.returnHeat.ReturnOpponentHeat(place) - 1;  // 자기 자신의 과열도 삐기
        heatPreferPoint = CalculateHeatPreferPoint(piece, place, deltaHeat);

        //안정도 점수
        // 안정도 점수 = (과열도 선호 점수 / (1 + 과열도 선호 점수))
        // 0, 1/2, 2/3, ...
        float tempPieceScore = 1 - ((float)piece.PieceScore / (1 + piece.PieceScore));
        tempPieceScore *= 3;
        heatPreferPoint += tempPieceScore;
        placePreferPoint = heatPreferPoint / (1 + heatPreferPoint);

        extentPoint = CalculateExtentPoint(piece, place);
        extentPoint *= willingToExtend;

        // 이동해본다음에 계산해야한다.
        assumePoint = CalculateAssumeMoveScore(piece, place);
        assumePoint *= futureOriented;
        //assumePoint = 0;
        if (GameManager.Instance.scoreDebugMode) Debug.Log("가정 점수: " + assumePoint);

        mindPoint = Random.Range(0, 3);
        mindPoint = mindPoint / (1 + mindPoint);
        mindPoint *= floatingMind;

        score = attackPoint + placePreferPoint + assumePoint + extentPoint + mindPoint;
        if (GameManager.Instance.scoreDebugMode)
            Debug.Log(place.boardIndex + " == 공격 점수: " + attackPoint + " / 이동 점수: " + placePreferPoint + " /  가정 점수: " + assumePoint + 
                " / 확장 점수: " + extentPoint + "/ 랜덤 점수: " + mindPoint);

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
        
        // 한번 움직인 상황임을 전제한다.
        piece.MoveCount++;


        // 기물이 움직였다고 가정한 후, 기물의 움직임대로 영향권 위치를 계산한다.
        // 기물의 기존 영향 점수는 영향을 미치지 않는가?
        piece.place.Piece = null;
        piece.MovePattern.RecognizeRange(location, assumeStateLists);

        // 이동할 수 있는 칸 수
        // + 이동할 수 있는 칸 중 최대 과열도?
        // 혹은 팀 과열도가 부족한 곳? - 백업 우선
        // 혹은 팀 과열도가 더 많은 곳? - 안정 우선
        int movableCount = 0;
        int maxHeatPoint = -99;

        for (int i = 0; i < assumeStateLists.movable.Count; i++)
        {
            Place assumePlace = assumeStateLists.movable[i];

            // 위치 선호도를 함께 고려한다. - 위치 선호 전략에 따른다.
            int deltaHeatPoint = piece.returnHeat.ReturnTeamHeat(assumePlace) - piece.returnHeat.ReturnOpponentHeat(assumePlace);
            // 문제: 자신의 과열도가 함께 고려되고 있다.

            movableCount += 1;

            if (maxHeatPoint < deltaHeatPoint)
                maxHeatPoint = deltaHeatPoint;  // 확장 점수에 보너스
        }

        // 이동 범위 확장 
        // 확장 점수 = 갈 수 있는 위치 개수 / (1 + 기물 점수)
        extendablePoint = (float)movableCount / (piece.PieceScore * piece.PieceScore);

        extendablePoint *= willingToExtend;
        if (GameManager.Instance.scoreDebugMode) Debug.Log("확장 점수: " + extendablePoint);

        if (piece.returnHeat.ReturnOpponentHeat(place) > 0)
        {
            heatPreferPoint *= 0.5f;
        }

        // 공격할 수 있는 기물
        for (int i = 0; i < assumeStateLists.threating.Count; i++)
        {
            // 공격 점수 식에 따라 공격 점수를 반환받는다.
            Piece threatablePiece = assumeStateLists.threating[i];

            if (threatablePiece.PieceScore < piece.PieceScore) continue;

            // 위협 점수 = 공격할 수 있는 기물들에 대한 공격 점수의 합
            threatablePoint += (float)(threatablePiece.PieceScore) / (1 + threatablePiece.PieceScore);
        }

        // 위협은 자신이 위험에 노출되는 상황이기도 하다. - 하지만 다음 이동에 관한 것이므로 아직 고려할 필요 없다.
        threatablePoint *= willingToThreat;

        // 방어할 수 있는 기물
        for (int i = 0; i < assumeStateLists.defending.Count; i++)
        {
            Piece defendablePiece = assumeStateLists.defending[i];
            Debug.Log("방어 대상 기물: " + defendablePiece);
            // 가정된 상황에서 자신을 방어하는 경우를 빼야 한다. - 기물을 임시 이동 시켰다.

            int targetDeltaHeat = piece.returnHeat.ReturnTeamHeat(defendablePiece.place) - piece.returnHeat.ReturnOpponentHeat(defendablePiece.place);
            int backUpPoint;
            if (targetDeltaHeat < 0) backUpPoint = 2;
            if (targetDeltaHeat == 0) backUpPoint = 1;
            else backUpPoint = 0;

            defendablePoint += ((float)(defendablePiece.PieceScore) / (1 + defendablePiece.PieceScore)) * backUpPoint; // * 과열도의 긴박한 정도. (과열도 차가 작을 수록 긴박함?)

            // 구원이 필요한 정도 + 구원하고자 하는 의지 (합산? 곱?)
        }

        defendablePoint *= willingToDefend;

        assumScore = /*extendablePoint +*/ heatPreferPoint + threatablePoint + defendablePoint;
        if(GameManager.Instance.scoreDebugMode)
            Debug.Log(string.Format("{0}에서부터 가정된 위치 점수 {5}-------\n 영향권: {1}, 과열도 안정적 점수: {2}, 위협: {3}, 보호 {4}",
            location, extendablePoint, heatPreferPoint, threatablePoint, defendablePoint, assumScore));


        // 되돌려 놓기
        piece.place.Piece = piece;
        piece.MoveCount--;
        return assumScore;


    }

    private float CalculateAttackPoint(Piece targetPiece)
    {
        float attackPoint = 0;

        if (targetPiece != null)         // 공격 가능한 장소라면
        {
            // 기물의 점수를 확인한다.
            // 기물에 대한 적의를 확인한다. 적개심만큼 곱한다.

            // 기물을 공격한 후에 대해 판단한다.

            attackPoint = (float)targetPiece.PieceScore / (1 + targetPiece.PieceScore);

        }
        if (GameManager.Instance.scoreDebugMode) Debug.Log("공격점수: " + attackPoint);

        return attackPoint;
    }

    private float CalculateHeatPreferPoint(Piece piece, Place place, int deltaHeat)
    {
        // 과열도 점수?
        // 과열도가 높은 곳에 갈 것인가?
        // 과열도가 낮은 곳에 갈 것인가?

        //안정 우선:

        // 상대팀 위협이 하나라도 있으면 우선도가 떨어지는 경우
        // 상대팀 위협이 있어도, 중요도가 높다면(?), 팀을 믿고 이동하는 경우

        // 먹을 수 있는 기물의 경우에는... 그 기물의 과열도를 하나 빼야 한다.
        
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
            Debug.Log("멀리 가기 점수: " + extentPoint);

        return extentPoint;
    }
}
