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
        // ������ �� ĭ �̵��� �� �ִ°�?
        // ���̶��
        if (IsTopLocation(curLocation, boardHeight)) return;

        // �⹰�� �ִٸ� ����, �⹰�� ���ٸ� �̵��� �� �ִ� ������ ���
        if (RecognizePieceMoveObstacle(curLocation)) return;

        // �⹰�� ����, �ι� �����̴� ������ �����ȴٸ� �ѹ� �� Ȯ��
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
        // ������ �� ĭ �̵��� �� �ִ°�?
        if (canDoubleMove)
        {
            // ���̶��
            if (IsTopLocation(curLocation, boardHeight)) return;

            // �⹰�� �ִٸ� ����, �⹰�� ���ٸ� �̵��� �� �ִ� ������ ���
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
