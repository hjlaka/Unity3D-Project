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

        if (targetPiece != null)         // 공격 가능한 장소라면
        {
            // 기물의 점수를 확인한다.
            // 기물에 대한 적의를 확인한다. 적개심만큼 곱한다.

            // 기물을 공격한 후에 대해 판단한다.
            // 안전 점수는 언제 계산할까?
            attackPoint += targetPiece.PieceScore;

            attackPoint *= willingToAttack;
        }
        // 과열도 점수?
        // 과열도가 높은 곳에 갈 것인가?
        // 과열도가 낮은 곳에 갈 것인가?

        //안정 우선:
        heatPreferPoint = piece.returnHeat.ReturnTeamHeat(place) - piece.returnHeat.ReturnOpponentHeat(place) - 1;  // 자기 자신의 과열도 삐기

        // 이동해본다음에 계산해야한다.
        assumePoint = CalculateAssumeMoveScore(piece, place);
        assumePoint *= futureOriented;
        
        score = attackPoint + heatPreferPoint + assumePoint;
        Debug.Log(place.boardIndex + " == 공격 점수: " + attackPoint + " / 이동 점수: " + heatPreferPoint + " /  가정 점수: " + assumePoint);
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



        // 기물이 움직였다고 가정한 후, 기물의 움직임대로 영향권을 계산한다.
        piece.place.piece = null;
        piece.MovePattern.RecognizeRange(location, assumeStateLists);

        // 이동할 수 있는 칸 수
        // + 이동할 수 있는 칸 중 최대 과열도?
        // 혹은 팀 과열도가 부족한 곳? - 백업 우선
        // 혹은 팀 과열도가 더 많은 곳? - 안정 우선
        for (int i = 0; i < assumeStateLists.movable.Count; i++)
        {
            Place assumePlace = assumeStateLists.movable[i];
            float deltaHeatPoint = piece.returnHeat.ReturnTeamHeat(assumePlace) - piece.returnHeat.ReturnOpponentHeat(assumePlace);
            // 문제: 자신의 과열도가 함께 고려되고 있다.

            influencablePoint += 1;

            if (heatPreferPoint < deltaHeatPoint)
                heatPreferPoint = deltaHeatPoint;
        }

        influencablePoint *= willingToExtend;

        heatPreferPoint *= willingToSafe;

        // 공격할 수 있는 기물
        for (int i = 0; i < assumeStateLists.threating.Count; i++)
        {
            Piece threatablePiece = assumeStateLists.threating[i];
            threatablePoint += threatablePiece.PieceScore;
        }

        threatablePoint *= (willingToThreat - willingToSafe);

        // 방어할 수 있는 기물
        for (int i = 0; i < assumeStateLists.defending.Count; i++)
        {
            Piece defendablePiece = assumeStateLists.defending[i];
            Debug.Log("방어 대상 기물: " + defendablePiece);
            // 가정된 상황에서 자신을 방어하는 경우를 빼야 한다.
            defendablePoint += defendablePiece.PieceScore; // + 과열도의 긴박한 정도. (과열도 차가 작을 수록 긴박함?)
        }

        defendablePoint *= willingToDefend;

        assumScore = influencablePoint + heatPreferPoint + threatablePoint + defendablePoint;
        Debug.Log(string.Format("{0}에서부터 가정된 위치 점수 {5}-------\n 영향권: {1}, 과열도 안정적 점수: {2}, 위협: {3}, 보호 {4}",
            location, influencablePoint, heatPreferPoint, threatablePoint, defendablePoint, assumScore));


        // 되돌려 놓기
        piece.place.piece = piece;
        return assumScore;


    }


    protected void AddScoreToLocation(Vector2Int location, int score)
    {
        scores[location.x, location.y] = score;
    }

}
