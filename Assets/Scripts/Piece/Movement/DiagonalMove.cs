using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalMove : MoveRecognizer, IPieceMovable, IGradable
{
    private int level;
    private int validMoveCount;
    private int curMoveCount;

    // 단계를 나눠서 강화되게 하기?
    public DiagonalMove(Piece controlled) : base(controlled)
    {
        // Do Nothing
    }
    public void RecognizeRange(Vector2Int location, StateLists recognized)
    {
        Vector2Int boardSize = controlled.place.board.Size;
        recognizedLists = recognized;

        DiagonalLB(location + new Vector2Int(-1, -1));
        DiagonalLT(location + new Vector2Int(-1, 1), boardSize.y);
        DiagonalRB(location + new Vector2Int(1, -1), boardSize.x);
        DiagonalRT(location + new Vector2Int(1, 1), boardSize.y, boardSize.x);
    }

    private void DiagonalLT(Vector2Int curLocation, int boardHeight)
    {

        if (curLocation.x < 0) return;
        if (curLocation.y > boardHeight - 1) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;


        DiagonalLT(curLocation + new Vector2Int(-1, 1), boardHeight);
    }

    private void DiagonalLB(Vector2Int curLocation)
    {
        if (curLocation.x < 0) return;
        if (curLocation.y < 0) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;

        DiagonalLB(curLocation + new Vector2Int(-1, -1));
    }

    private void DiagonalRT(Vector2Int curLocation, int boardHeight, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;
        if (curLocation.y > boardHeight - 1) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;

        DiagonalRT(curLocation + new Vector2Int(1, 1), boardHeight, boardWidth);
    }

    private void DiagonalRB(Vector2Int curLocation, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;
        if (curLocation.y < 0) return;

        // 이동 가능 범위 등록
        if (RecognizePiece(curLocation)) return;

        DiagonalRB(curLocation + new Vector2Int(1, -1), boardWidth);
    }

    public void RecognizeSpecialMove(Place newPlace)
    {
        // Do Nothing
    }

    public void LevelUp()
    {
        level++;
    }
}
