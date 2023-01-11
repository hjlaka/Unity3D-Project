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

        if (IsTopOutLocation(curLocation, boardHeight)) return;
        if (IsBottomOutLocation(curLocation)) return;

        if (RecognizeObstaclePiece(curLocation)) return;

        // 기물이 없고, 두번 움직이는 조건이 충족된다면 한번 더 확인
        MoveDoubleForward(curLocation + new Vector2Int(0, 1), boardHeight);
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
        // 앞으로 두 칸 이동할 수 있는가?
        if (canDoubleMove)
        {
            // 벽이라면
            if (IsTopOutLocation(curLocation, boardHeight)) return;

            // 기물이 있다면 종료, 기물이 없다면 이동할 수 있는 범위로 등록
            if (RecognizeObstaclePiece(curLocation)) return;

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
