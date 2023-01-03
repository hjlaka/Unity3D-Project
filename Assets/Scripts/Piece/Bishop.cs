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

        DiagonalLB(location + new Vector2Int(-1, -1));
        DiagonalLT(location + new Vector2Int(-1, 1), boardSize.y);
        DiagonalRB(location + new Vector2Int(1, -1), boardSize.x);
        DiagonalRT(location + new Vector2Int(1, 1), boardSize.y, boardSize.x);

        return false;
    }

    private void DiagonalLT(Vector2Int curLocation, int boardHeight)
    {

        if(curLocation.x < 0) return;
        if (curLocation.y > boardHeight - 1) return;

        // 이동 가능 범위 등록
        RecognizePiece(curLocation);


        DiagonalLT(curLocation + new Vector2Int(-1, 1), boardHeight);
    }

    private void DiagonalLB(Vector2Int curLocation)
    {
        if (curLocation.x < 0) return;
        if (curLocation.y < 0) return;

        // 이동 가능 범위 등록
        //PlaceManager.Instance.ChangePlaceColor(curLocation);
        RecognizePiece(curLocation);

        DiagonalLB(curLocation + new Vector2Int(-1, -1));
    }

    private void DiagonalRT(Vector2Int curLocation, int boardHeight, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;
        if (curLocation.y > boardHeight - 1) return;

        // 이동 가능 범위 등록
        //PlaceManager.Instance.ChangePlaceColor(curLocation);
        RecognizePiece(curLocation);

        DiagonalRT(curLocation + new Vector2Int(1, 1), boardHeight, boardWidth);
    }

    private void RecognizePiece(Vector2Int curLocation)
    {
        Piece targetPiece = this.place.board.places[curLocation.x, curLocation.y].piece;
        if (targetPiece != null)
        {
            if (targetPiece.team.TeamId == team.TeamId)
            {
                AddDefence(targetPiece);
                targetPiece.BeDefended(this);
                PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.DEFENCE);
                DialogueManager.Instance.ShowDialogueUI("Defend" + targetPiece);
            }
            else
            {
                AddThreat(targetPiece);
                targetPiece.BeThreatened(this);
                PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.ATTACK);
                DialogueManager.Instance.ShowDialogueUI("Attack" + targetPiece);
            }
        }
        else
        {
            PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.MOVABLE);
        }
    }

    private void DiagonalRB(Vector2Int curLocation, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1) return;
        if (curLocation.y < 0) return;

        // 이동 가능 범위 등록
        //PlaceManager.Instance.ChangePlaceColor(curLocation);
        Piece targetPiece = this.place.board.places[curLocation.x, curLocation.y].piece;
        if (targetPiece != null)
        {
            if (targetPiece.team.TeamId == team.TeamId)
            {
                AddDefence(targetPiece);
                targetPiece.BeDefended(this);
                PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.DEFENCE);
            }
            else
            {
                AddThreat(targetPiece);
                targetPiece.BeThreatened(this);
                PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.ATTACK);
            }
        }
        else
        {
            PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.MOVABLE);
        }

        DiagonalRB(curLocation + new Vector2Int(1, -1), boardWidth);
    }

    private void Influence()
    {

    }

}
