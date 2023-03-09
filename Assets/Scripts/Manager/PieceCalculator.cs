using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCalculater : MonoBehaviour
{
    public void CalculateInfluence(Piece piece)
    {
        Place newPlace = piece.place;
        Board curBoard = newPlace.board;

        // 기물이 있는 곳이 보드가 아니라면 종료, 규칙을 따르지 않는 보드라면 종료
        if (null == curBoard || !curBoard.FollowRule)
            return;

        piece.RecognizeRange(piece.place.boardIndex);
    }
}
