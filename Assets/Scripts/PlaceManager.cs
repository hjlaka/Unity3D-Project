using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : SingleTon<PlaceManager>
{
    public Piece selectedPiece;

    public Place selectedPlace;

    public Place[,] places;

    [SerializeField]
    private Color highlight;


    public void ShowPlaceable()
    {
        Place curPlace = selectedPiece.place;

        for(int i = 0; i < places.GetLength(0); i++)
        {
            for (int j = 0; j < places.GetLength(1); j++)
            {
                if (places[i, j].piece != null)
                    continue;

                places[i, j].ChangeColor(highlight);
            }

        }
    }

    public void ShowPlaceableEnd()
    {
        for (int i = 0; i < places.GetLength(0); i++)
        {
            for (int j = 0; j < places.GetLength(1); j++)
            {
                places[i, j].ChangeColor();
            }

        }
    }

    public void MovePieceTo(Place place)
    {
        Place oldPlace = selectedPiece.place;
        oldPlace.piece = null;

        selectedPiece.SetInPlace(place);
        place.piece = selectedPiece;
        

        ControlInit();
    }

    private void ControlInit()
    {
        selectedPiece = null;
        ShowPlaceableEnd();
    }
}
