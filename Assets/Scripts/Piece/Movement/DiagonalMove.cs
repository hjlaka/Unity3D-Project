using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalMove : MoveRecognizer, IPieceMovable, IGradable
{
    private int level;
    private int validMoveCount;
    private int curMoveCount;

    // �ܰ踦 ������ ��ȭ�ǰ� �ϱ�?
    public DiagonalMove(Piece controlled, int level) : base(controlled)
    {
        this.level = level;

        switch(level)
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
        Vector2Int boardSize = controlled.place.board.Size;
        recognizedLists = recognized;

        curMoveCount = 0;

        DiagonalLB(location + new Vector2Int(-1, -1));
        DiagonalLT(location + new Vector2Int(-1, 1), boardSize.y);
        DiagonalRB(location + new Vector2Int(1, -1), boardSize.x);
        DiagonalRT(location + new Vector2Int(1, 1), boardSize.y, boardSize.x);
    }

    private void DiagonalLT(Vector2Int curLocation, int boardHeight)
    {

        if (curLocation.x < 0) return;
        if (curLocation.y > boardHeight - 1) return;

        // �̵� ���� ���� ���
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;


        DiagonalLT(curLocation + new Vector2Int(-1, 1), boardHeight);
    }

    private void DiagonalLB(Vector2Int curLocation)
    {
        if (curLocation.x < 0) return;
        if (curLocation.y < 0) return;

        // �̵� ���� ���� ���
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;

        DiagonalLB(curLocation + new Vector2Int(-1, -1));
    }

    private void DiagonalRT(Vector2Int curLocation, int boardHeight, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;
        if (curLocation.y > boardHeight - 1) return;

        // �̵� ���� ���� ���
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;

        DiagonalRT(curLocation + new Vector2Int(1, 1), boardHeight, boardWidth);
    }

    private void DiagonalRB(Vector2Int curLocation, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;
        if (curLocation.y < 0) return;

        // �̵� ���� ���� ���
        if (RecognizePiece(curLocation)) return;

        curMoveCount++;

        if (curMoveCount >= validMoveCount) return;

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
