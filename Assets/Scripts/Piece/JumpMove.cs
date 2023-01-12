using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMove : MoveRecognizer, IPieceMovable
{
    public JumpMove(Piece controlled) : base(controlled)
    {
        // Do Nothing
    }

    public void RecognizeRange(Vector2Int location)
    {
        Vector2Int boardSize = controlled.place.board.Size;

        JumpPosition(location + new Vector2Int(2, 1), boardSize.y, boardSize.x);
        JumpPosition(location + new Vector2Int(2, -1), boardSize.y, boardSize.x);
        JumpPosition(location + new Vector2Int(-2, 1), boardSize.y, boardSize.x);
        JumpPosition(location + new Vector2Int(-2, -1), boardSize.y, boardSize.x);
        JumpPosition(location + new Vector2Int(1, 2), boardSize.y, boardSize.x);
        JumpPosition(location + new Vector2Int(-1, 2), boardSize.y, boardSize.x);
        JumpPosition(location + new Vector2Int(1, -2), boardSize.y, boardSize.x);
        JumpPosition(location + new Vector2Int(-1, -2), boardSize.y, boardSize.x);
    }

    private void JumpPosition(Vector2Int curLocation, int boardHeight, int boardWidth)
    {
        if (curLocation.x < 0 || curLocation.x > boardWidth - 1) return;
        if (curLocation.y < 0 || curLocation.y > boardHeight - 1) return;

        if (RecognizePiece(curLocation)) return;
    }
    public void RecognizeSpecialMove(Place newPlace)
    {
        // Do Nothing
    }
}
