using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCalculater : MonoBehaviour
{
    public void CalculateInfluence(Piece piece)
    {
        Place newPlace = piece.place;
        Board curBoard = newPlace.board;

        // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����, ��Ģ�� ������ �ʴ� ������ ����
        if (null == curBoard || !curBoard.FollowRule)
            return;

        piece.RecognizeRange(piece.place.boardIndex);
    }
}
