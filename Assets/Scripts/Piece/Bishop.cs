using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{

    public override void PieceAction()
    {
        base.PieceAction();

    }

    public override void LookAttackRange() 
    {

    }
    public override bool IsMovable(Vector2Int location)
    {

        Vector2Int boardSize = place.board.Size;

        DiagonalLB(location);
        DiagonalLT(location, boardSize.y);
        DiagonalRB(location, boardSize.x);
        DiagonalRT(location, boardSize.y, boardSize.x);

        return false;
        /*if (Mathf.Abs(location.x - place.boardIndex.x) == Mathf.Abs(location.y - place.boardIndex.y))
        {
            //Debug.Log("[" + (location.x - place.boardIndex.x) + "][" + (location.y - place.boardIndex.y) + "]");
            return true;
        }

        else
            return false;*/
    }

    private void DiagonalLT(Vector2Int curLocation, int boardHeight)
    {

        if(curLocation.x < 0) return;
        if (curLocation.y > boardHeight - 1) return;

        // 이동 가능 범위 등록
        PlaceManager.Instance.ChangePlaceColor(curLocation);

        DiagonalLT(curLocation + new Vector2Int(-1, 1), boardHeight);
    }

    private void DiagonalLB(Vector2Int curLocation)
    {
        if (curLocation.x < 0) return;
        if (curLocation.y < 0) return;

        // 이동 가능 범위 등록
        PlaceManager.Instance.ChangePlaceColor(curLocation);

        DiagonalLB(curLocation + new Vector2Int(-1, -1));
    }

    private void DiagonalRT(Vector2Int curLocation, int boardHeight, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;
        if (curLocation.y > boardHeight - 1) return;

        // 이동 가능 범위 등록
        PlaceManager.Instance.ChangePlaceColor(curLocation);

        DiagonalRT(curLocation + new Vector2Int(1, 1), boardHeight, boardWidth);
    }

    private void DiagonalRB(Vector2Int curLocation, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;
        if (curLocation.y < 0) return;

        // 이동 가능 범위 등록
        PlaceManager.Instance.ChangePlaceColor(curLocation);

        DiagonalRB(curLocation + new Vector2Int(1, -1), boardWidth);
    }

    private void Influence()
    {

    }

}
