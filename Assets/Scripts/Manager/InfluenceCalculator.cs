using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceCalculator : MonoBehaviour
{
    public void CalculateInfluence(Piece piece)
    {
        Place newPlace = piece.place;
        Board curBoard = newPlace.board;

        if (null == curBoard)
            return;

        if (!curBoard.FollowRule)
            return;

        piece.RecognizeRange(piece.place.boardIndex);
    }

    public void ApplyInfluence(Piece piece)
    {
        Place curPlace = piece.place;
        List<Place> iterPlaces = piece.Recognized.influenceable;

        curPlace.AddInfluence(piece.team.direction);
        
        for (int i = 0; i < iterPlaces.Count; i++)
        {
            iterPlaces[i].AddInfluence(piece.team.direction);
            iterPlaces[i].registerObserver(piece.PlaceObserver);
        }

        for (int i = 0; i < piece.Recognized.special.Count; i++)
        {
            Place iterPlace = piece.Recognized.special[i];

            iterPlace.registerObserver(piece.PlaceObserver);
        }
    }

    public void ReCalculateInfluence(Piece piece)
    {
        Debug.Log(piece + "영향 재계산");
        InitInfluence(piece);
        CalculateInfluence(piece);
        ApplyInfluence(piece);
    }

    public void WithDrawInfluence(Piece leftPiece)
    {
        Place leftPlace = leftPiece.place;
        List<Place> influencable = leftPiece.Recognized.influenceable;

        if (leftPlace == null) return;

        leftPlace.SubInfluence(leftPiece.team.direction);

        for (int i = 0; i < influencable.Count; i++)
        {
            influencable[i].SubInfluence(leftPiece.team.direction);
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
