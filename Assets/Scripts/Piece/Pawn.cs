using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pawn : Piece
{
    [SerializeField]
    private bool canDoubleMove;

    public override void PieceAction()
    {
        base.PieceAction();

    }

    public override void RecognizeRange(Vector2Int location)
    {
        Vector2Int boardSize = place.board.Size;

        MoveForward(location + new Vector2Int(0, forwardY), boardSize.y);

        AttackDiagonalLT(location + new Vector2Int(-1, forwardY), boardSize.y);
        AttackDiagonalRT(location + new Vector2Int(1, forwardY), boardSize.y, boardSize.x);
    }

    private void MoveForward(Vector2Int curLocation, int boardHeight)
    {

        if (recognizer.IsTopOutLocation(curLocation, boardHeight)) return;
        if (recognizer.IsBottomOutLocation(curLocation)) return;

        if (recognizer.RecognizeObstaclePiece(curLocation)) return;

        // �⹰�� ����, �ι� �����̴� ������ �����ȴٸ� �ѹ� �� Ȯ��
        MoveDoubleForward(curLocation + new Vector2Int(0, 1), boardHeight);
    }

    private void AttackDiagonalLT(Vector2Int curLocation, int boardHeight)
    {
        if (recognizer.IsLeftOutLocation(curLocation) ||
            recognizer.IsTopOutLocation(curLocation, boardHeight) ||
            recognizer.IsBottomOutLocation(curLocation)) 
            return;

        recognizer.RecognizePieceOnlyInfluence(curLocation);
    }

    private void AttackDiagonalRT(Vector2Int curLocation, int boardHeight, int boardWidth)
    {
        if (recognizer.IsRightOutLocation(curLocation, boardWidth) ||
            recognizer.IsTopOutLocation(curLocation, boardHeight) ||
            recognizer.IsBottomOutLocation(curLocation))
            return;

        recognizer.RecognizePieceOnlyInfluence(curLocation);
    }

    private void MoveDoubleForward(Vector2Int curLocation, int boardHeight)
    {
        // ������ �� ĭ �̵��� �� �ִ°�?
        if (canDoubleMove)
        {
            // ���̶��
            if (recognizer.IsTopOutLocation(curLocation, boardHeight)) return;

            // �⹰�� �ִٸ� ����, �⹰�� ���ٸ� �̵��� �� �ִ� ������ ���
            if (recognizer.RecognizeObstaclePiece(curLocation)) return;

        }
    }

    public override void SetInPlace(Place place)
    {
        Place oldPlace = this.place;
        if (oldPlace?.board != place.board)
            canDoubleMove = true;
        else
            canDoubleMove = false;

        base.SetInPlace(place);
    }

    public bool IsAttackable()
    {
        return false;
    }
}
