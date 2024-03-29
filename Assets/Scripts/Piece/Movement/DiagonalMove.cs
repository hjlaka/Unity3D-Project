using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalMove : MoveRecognizer, IGradable
{
    private int level;
    private int validMoveCount;
    private int curMoveCount;

    Observer observerLT;

    ReachForPiece reachedLT;
    ReachForPiece reachedRT;
    ReachForPiece reachedLB;
    ReachForPiece reachedRB;


    // 단계를 나눠서 강화되게 하기?
    public DiagonalMove(Piece controlled, int level) : base(controlled)
    {
        this.level = level;

        reachedLT = new ReachForPiece();
        reachedRT = new ReachForPiece();
        reachedLB = new ReachForPiece();
        reachedRB = new ReachForPiece();

        ApplyLevel();
    }

    private void ApplyLevel()
    {
        switch (level)
        {
            case 1:
                validMoveCount = 1;
                break;
            case 2:
                validMoveCount = 3;
                break;
            case 3:
                validMoveCount = 99;
                break;
            default:
                break;
        }
    }
    public override void RecognizeRange(Vector2Int location, StateLists recognized)
    {
        Vector2Int boardSize = controlled.place.board.Size;
        recognizedLists = recognized;

        curMoveCount = 0;

        DiagonalLB(location + new Vector2Int(-1, -1));
        DiagonalLT(location + new Vector2Int(-1, 1), boardSize.y);
        DiagonalRB(location + new Vector2Int(1, -1), boardSize.x);
        DiagonalRT(location + new Vector2Int(1, 1), boardSize.y, boardSize.x);
    }

    private bool DiagonalLT(Vector2Int curLocation, int boardHeight)
    {
        // 허용된 위치인지 탐색
        if (curLocation.x < 0) return false;
        if (curLocation.y > boardHeight - 1) return false;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return true;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return false;

        DiagonalLT(curLocation + new Vector2Int(-1, 1), boardHeight);

        return false;

        // movable에 등록?
    }

    private void DiagonalLB(Vector2Int curLocation)
    {
        if (curLocation.x < 0) return;
        if (curLocation.y < 0) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;

        DiagonalLB(curLocation + new Vector2Int(-1, -1));
    }

    private void DiagonalRT(Vector2Int curLocation, int boardHeight, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;
        if (curLocation.y > boardHeight - 1) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;

        DiagonalRT(curLocation + new Vector2Int(1, 1), boardHeight, boardWidth);
    }

    private void DiagonalRB(Vector2Int curLocation, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;
        if (curLocation.y < 0) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;

        DiagonalRB(curLocation + new Vector2Int(1, -1), boardWidth);
    }

    public override void RecognizeSpecialMove(Place newPlace)
    {
        // Do Nothing
    }

    public void LevelUp()
    {
        level++;
    }
}
