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
        // ù��° ������ : ������ �� ĭ �̵�
        if (location.x == place.boardIndex.x && location.y == place.boardIndex.y + 1)
            return true;

        // �ι�° ������ : ������ �� ĭ �̵�
        else if (location.x == place.boardIndex.x && location.y == place.boardIndex.y + 2)
            return true;
        // ����° ������ : ���� ����

        // �� �ٸ� ���� �� : ���� ���� �����ߴ°�?
        return false;
    }

    private void MoveOnce()
    {
        // ������ �� ĭ �̵�
    }

    private void MoveTwice()
    {
        // ������ �� ĭ �̵�
    }

    public bool IsAttackable()
    {
        return false;
    }
}
