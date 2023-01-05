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



    public override void IsMovable(Vector2Int location)
    {
        Vector2Int boardSize = place.board.Size;

        MoveForward(location + new Vector2Int(0, 1), boardSize.y);
    }

    private void MoveForward(Vector2Int curLocation, int boardHeight)
    {
        // 앞으로 한 칸 이동할 수 있는가?
        // 벽이라면
        if (IsTopLocation(curLocation, boardHeight)) return;

        // 기물이 있다면 종료, 기물이 없다면 이동할 수 있는 범위로 등록
        if (RecognizePieceMoveObstacle(curLocation)) return;

        // 기물이 없고, 두번 움직이는 조건이 충족된다면 한번 더 확인
        MoveDoubleForward(curLocation + new Vector2Int(0, 1), boardHeight);
    }


    private bool IsTopLocation(Vector2Int curLocation, int boardHeight)
    {
        if (curLocation.y > boardHeight - 1) 
            return true;
        else 
            return false;
    }


    private void MoveDoubleForward(Vector2Int curLocation, int boardHeight)
    {
        // 앞으로 두 칸 이동할 수 있는가?
        if (canDoubleMove)
        {
            // 벽이라면
            if (IsTopLocation(curLocation, boardHeight)) return;

            // 기물이 있다면 종료, 기물이 없다면 이동할 수 있는 범위로 등록
            if (RecognizePieceMoveObstacle(curLocation)) return;

        }
    }

    public override void SetInPlace(Place place)
    {
        Place oldPlace = this.place;
        if (oldPlace.board != place.board)
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
