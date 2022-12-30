using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : SingleTon<PlaceManager>
{
    public Piece selectedPiece;

    public Place selectedPlace;

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
    }
}
