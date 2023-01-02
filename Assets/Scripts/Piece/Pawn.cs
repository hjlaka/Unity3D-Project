using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private bool isMoved;

    public override void PieceAction()
    {
        base.PieceAction();

    }



    public override bool IsMovable(Vector2Int location)
    {
        // 첫번째 움직임 : 앞으로 한 칸 이동
        if (location.x == place.boardIndex.x && location.y == place.boardIndex.y + 1)
            return true;

        // 두번째 움직임 : 앞으로 두 칸 이동
        else if (location.x == place.boardIndex.x && location.y == place.boardIndex.y + 2)
            return true;
        // 세번째 움직임 : 적군 공격

        // 또 다른 조건 식 : 판의 끝에 도달했는가?
        return false;
    }

    private void MoveOnce()
    {
        // 앞으로 한 칸 이동
    }

    private void MoveTwice()
    {
        // 앞으로 두 칸 이동
    }

    public bool IsAttackable()
    {
        return false;
    }
}
