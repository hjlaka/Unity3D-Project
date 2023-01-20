using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMove : MoveRecognizer, IPieceMovable
{

    private bool canDoubleMove;

    public PawnMove(Piece controlled, bool canDoubleMove = true) : base(controlled)
    {
        this.canDoubleMove = canDoubleMove;
    }

    public void RecognizeRange(Vector2Int location, StateLists recognized)
    {
        Vector2Int boardSize = controlled.place.board.Size;
        recognizedLists = recognized;

        MoveForward(location + new Vector2Int(0, controlled.forwardY), boardSize.y);

        AttackDiagonalLT(location + new Vector2Int(-1, controlled.forwardY), boardSize.y);
        AttackDiagonalRT(location + new Vector2Int(1, controlled.forwardY), boardSize.y, boardSize.x);
    }

    public void RecognizeSpecialMove(Place newPlace)
    {
        Place oldPlace = controlled.place;
        if (oldPlace?.board != newPlace.board)
            canDoubleMove = true;
        else
            canDoubleMove = false;
        /*            Debug.Log("Ư�� �̵� üũ: " + canDoubleMove);
                if (canDoubleMove && controlled.MoveCount > 0)
                    canDoubleMove = false;
                Debug.Log("Ư�� �̵� üũ2: " + canDoubleMove);
        */
    }
    // �߰��� �⹰�� ���� �Ǹ� �ι� �������� ����� ���ΰ�?
    // ���� ����? ������ �࿡���� �ι� ������ �� �ְ� �ϱ�?

    private void MoveForward(Vector2Int curLocation, int boardHeight)
    {

        if (IsTopOutLocation(curLocation, boardHeight)) return;
        if (IsBottomOutLocation(curLocation)) return;

        if (RecognizeObstaclePiece(curLocation)) return;

        // �⹰�� ����, �ι� �����̴� ������ �����ȴٸ� �ѹ� �� Ȯ��
        MoveDoubleForward(curLocation + new Vector2Int(0, controlled.forwardY), boardHeight);
    }

    private void AttackDiagonalLT(Vector2Int curLocation, int boardHeight)
    {
        if (IsLeftOutLocation(curLocation) ||
            IsTopOutLocation(curLocation, boardHeight) ||
            IsBottomOutLocation(curLocation))
            return;

        RecognizePieceOnlyInfluence(curLocation);
    }

    private void AttackDiagonalRT(Vector2Int curLocation, int boardHeight, int boardWidth)
    {
        if (IsRightOutLocation(curLocation, boardWidth) ||
            IsTopOutLocation(curLocation, boardHeight) ||
            IsBottomOutLocation(curLocation))
            return;

        RecognizePieceOnlyInfluence(curLocation);
    }


    private void MoveDoubleForward(Vector2Int curLocation, int boardHeight)
    {
        // ������ �� ĭ �̵��� �� �ִ°�?
        if (canDoubleMove)
        {
            // ���̶��
            if (IsTopOutLocation(curLocation, boardHeight)) return;
            if (IsBottomOutLocation(curLocation)) return;

            // �⹰�� �ִٸ� ����, �⹰�� ���ٸ� �̵��� �� �ִ� ������ ���
            if (RecognizeObstaclePiece(curLocation)) return;

        }
    }



}
