using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMove : MoveRecognizer, IPieceMovable
{
    private int level;
    private int validMoveCount;
    private int curMoveCount;
    public StraightMove(Piece controlled, int level) : base(controlled)
    {
        this.level = level;

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

    public void RecognizeRange(Vector2Int location, StateLists recognized)
    {
        Vector2Int boardSize = controlled.place.board.Size;     // 기물이 있는 보드에서 판단을 한다고 가정함.
        recognizedLists = recognized;

        curMoveCount = 0;

        StraightB(location + new Vector2Int(0, -1));
        StraightT(location + new Vector2Int(0, 1), boardSize.y);
        StraightL(location + new Vector2Int(-1, 0));
        StraightR(location + new Vector2Int(1, 0), boardSize.x);
    }
    private void StraightT(Vector2Int curLocation, int boardHeight)
    {

        if (curLocation.y > boardHeight - 1) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;


        StraightT(curLocation + new Vector2Int(0, 1), boardHeight);
    }

    private void StraightB(Vector2Int curLocation)
    {
        if (curLocation.y < 0) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;

        StraightB(curLocation + new Vector2Int(0, -1));
    }

    private void StraightL(Vector2Int curLocation)
    {
        if (curLocation.x < 0) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;

        StraightL(curLocation + new Vector2Int(-1, 0));
    }

    private void StraightR(Vector2Int curLocation, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;

        StraightR(curLocation + new Vector2Int(1, 0), boardWidth);
    }

    public void RecognizeSpecialMove(Place newPlace)
    {
        // Do Nothing
    }
}
