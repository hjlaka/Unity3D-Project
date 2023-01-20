using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecidePlaceStrategy
{
    protected ScoreMap scoreMap = new ScoreMap();

    protected IHeatPreperStrategy heatPreperStrategy;

    protected StrategyData strategyData;


    protected float willingToAttack = 4f;
    protected float willingToDefend = 1f;
    protected float willingToExtend = 0.5f;
    protected float willingToSafe = 1.3f;
    protected float willingToThreat = 1.2f;
    protected float futureOriented = 0.1f;

    protected float totalWeight;

    protected virtual void CopyData()
    {
        // override
    }

    protected float GetTotalWeight()
    {
        totalWeight = willingToAttack + willingToDefend + willingToExtend + willingToSafe + willingToThreat + futureOriented;

        return totalWeight;
    }


    protected virtual ScoreNode CalculateScore(Piece piece, Place place)
    {
        float score;
        float attackPoint = 0;
        float placePreferPoint;
        float defencePoint = 0;
        float assumePoint;
        //float futureOrientPoint;

        Piece targetPiece = place.Piece;

        if (targetPiece != null)         // 공격 가능한 장소라면
        {
            // 기물의 점수를 확인한다.
            // 기물에 대한 적의를 확인한다. 적개심만큼 곱한다.

            // 기물을 공격한 후에 대해 판단한다.
            // 안전 점수는 언제 계산할까?
            //attackPoint += targetPiece.PieceScore;

            //공격 점수 = (기물 점수 / (1 + 기물 점수))
            // 1/2, 3/4, 5/6, 9/10
            // 왕의 경우는?
            attackPoint = (float)targetPiece.PieceScore / (1 + targetPiece.PieceScore);

            attackPoint *= willingToAttack;


        }
        Debug.Log("공격점수: " + attackPoint);
        // 과열도 점수?
        // 과열도가 높은 곳에 갈 것인가?
        // 과열도가 낮은 곳에 갈 것인가?

        //안정 우선:

        // 상대팀 위협이 하나라도 있으면 우선도가 떨어지는 경우
        // 상대팀 위협이 있어도, 중요도가 높다면(?), 팀을 믿고 이동하는 경우

        // 먹을 수 있는 기물의 경우에는... 그 기물의 과열도를 하나 빼야 한다.
        int deltaHeat = piece.returnHeat.ReturnTeamHeat(place) - piece.returnHeat.ReturnOpponentHeat(place) - 1;  // 자기 자신의 과열도 삐기
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
        
        //안정도 점수
        // 안정도 점수 = (과열도 선호 점수 / (1 + 과열도 선호 점수))
        // 0, 1/2, 2/3, ...
        placePreferPoint = heatPreferPoint / (1 + heatPreferPoint);
        Debug.Log("위치 선호 점수: " + placePreferPoint);


        // 이동해본다음에 계산해야한다.
        assumePoint = CalculateAssumeMoveScore(piece, place);
        assumePoint *= futureOriented;
        //assumePoint = 0;
        Debug.Log("가정 점수: " + assumePoint);

        score = attackPoint + placePreferPoint + assumePoint;
        if (GameManager.Instance.scoreDebugMode)
            Debug.Log(place.boardIndex + " == 공격 점수: " + attackPoint + " / 이동 점수: " + placePreferPoint + " /  가정 점수: " + assumePoint);

        


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
        float heatPreferPoint = -99;


        StateLists assumeStateLists = new StateLists();

        Vector2Int location = place.boardIndex;



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
                maxHeatPoint = deltaHeatPoint;
        }

        // 이동 범위 확장 
        // 확장 점수 = 갈 수 있는 위치 개수 / (1 + 기물 점수)
        extendablePoint = (float)movableCount / (1 + movableCount);

        extendablePoint *= willingToExtend + (2 - ((float)piece.PieceScore / (1 + piece.PieceScore)));
        Debug.Log("확장 점수: " + extendablePoint);

        heatPreferPoint = (float)(heatPreferPoint) / (1 + heatPreferPoint);
        heatPreferPoint *= willingToSafe;

        // 공격할 수 있는 기물
        for (int i = 0; i < assumeStateLists.threating.Count; i++)
        {
            // 공격 점수 식에 따라 공격 점수를 반환받는다.
            Piece threatablePiece = assumeStateLists.threating[i];

            // 위협 점수 = 공격할 수 있는 기물들에 대한 공격 점수의 합
            threatablePoint += (float)(threatablePiece.PieceScore) / (1 + threatablePiece.PieceScore);
        }

        // 위협은 자신이 위험에 노출되는 상황이기도 하다.
        threatablePoint *= (willingToThreat - willingToSafe);

        // 방어할 수 있는 기물
        for (int i = 0; i < assumeStateLists.defending.Count; i++)
        {
            Piece defendablePiece = assumeStateLists.defending[i];
            Debug.Log("방어 대상 기물: " + defendablePiece);
            // 가정된 상황에서 자신을 방어하는 경우를 빼야 한다. - 기물을 임시 이동 시켰다.

            defendablePoint += (float)(defendablePiece.PieceScore) / (1 + defendablePiece.PieceScore); // * 과열도의 긴박한 정도. (과열도 차가 작을 수록 긴박함?)

            // 구원이 필요한 정도 + 구원하고자 하는 의지 (합산? 곱?)
        }

        defendablePoint *= willingToDefend;

        assumScore = /*extendablePoint +*/ heatPreferPoint + threatablePoint + defendablePoint;
        if(GameManager.Instance.scoreDebugMode)
            Debug.Log(string.Format("{0}에서부터 가정된 위치 점수 {5}-------\n 영향권: {1}, 과열도 안정적 점수: {2}, 위협: {3}, 보호 {4}",
            location, extendablePoint, heatPreferPoint, threatablePoint, defendablePoint, assumScore));


        // 되돌려 놓기
        piece.place.Piece = piece;
        return assumScore;


    }

}
