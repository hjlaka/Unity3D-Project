using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceCalculator : MonoBehaviour
{
    public void CalculateInfluence(Piece piece)
    {
        Place newPlace = piece.place;
        Board curBoard = newPlace.board;

        // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
        if (null == curBoard)
            return;

        // ��Ģ�� ������ �ʴ� ������ ����
        if (!curBoard.FollowRule)
            return;

        piece.RecognizeRange(piece.place.boardIndex);
    }

    public void ApplyInfluence(Piece piece)
    {
        Place curPlace = piece.place;
        // ���ο� �ڸ� ������ �߰�

        if (piece.team.direction == TeamData.Direction.DownToUp)
            curPlace.HeatPointBottomTeam++;
        else
            curPlace.HeatPointTopTeam++;

        // ��� �Ϸ�� ������� ������ �߰�
        for (int i = 0; i < piece.Recognized.influenceable.Count; i++)
        {
            Place iterPlace = piece.Recognized.influenceable[i];
            if (piece.team.direction == TeamData.Direction.DownToUp)
                iterPlace.HeatPointBottomTeam++;
            else
                iterPlace.HeatPointTopTeam++;

            iterPlace.registerObserver(piece.PlaceObserver);
        }

        for (int i = 0; i < piece.Recognized.special.Count; i++)
        {
            Place iterPlace = piece.Recognized.special[i];

            iterPlace.registerObserver(piece.PlaceObserver);
        }
    }

    public void ReCalculateInfluence(Piece piece)
    {
        Debug.Log(piece + "���� ����");
        InitInfluence(piece);
        CalculateInfluence(piece);
        ApplyInfluence(piece);
    }

    public void WithDrawInfluence(Piece leftPiece)
    {
        Place leftPlace = leftPiece.place;

        if (leftPlace == null) return;

        if (leftPiece.team.direction == TeamData.Direction.DownToUp)
        {
            leftPlace.HeatPointBottomTeam--;
        }
        else
        {
            leftPlace.HeatPointTopTeam--;
        }

        List<Place> influencable = leftPiece.Recognized.influenceable;
        for (int i = 0; i < influencable.Count; i++)
        {
            if (leftPiece.team.direction == TeamData.Direction.DownToUp)
            {
                influencable[i].HeatPointBottomTeam--;
            }
            else
            {
                influencable[i].HeatPointTopTeam--;
            }
            influencable[i].removeObserver(leftPiece.PlaceObserver);
        }

        for (int i = 0; i < leftPiece.Recognized.special.Count; i++)
        {
            Place curPlace = leftPiece.Recognized.special[i];

            curPlace.removeObserver(leftPiece.PlaceObserver);
        }
    }

    public void InitInfluence(Piece piece)
    {
        WithDrawInfluence(piece);

        piece.Recognized.ClearAllRecognized();

    }
}
