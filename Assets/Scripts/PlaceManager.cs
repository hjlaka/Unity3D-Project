using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceManager : SingleTon<PlaceManager>
{
    [SerializeField]
    private Piece selectedPiece;

    [SerializeField]
    private CameraController camController;

    public enum PlaceType { DEFENCE, ATTACK, MOVABLE, NORMAL, SPECIALMOVE}

    public Piece SelectedPiece
    {
        get { return selectedPiece; }
        set 
        { 
            selectedPiece = value;

            if (value != null && SelectedPiece.place.board?.heatPointHUD != null)
            {
                SelectedPiece.place.board.heatPointHUD.gameObject.SetActive(true);

            }
                
        }
    }

    public UnityEvent OnExitSelect;

    public Place selectedPlace;


    [SerializeField]
    private Color highlight;
    [SerializeField]
    private Color attackable;


    public void ShowHUD()
    {
        Transform hud = SelectedPiece.place.board.heatPointHUD;
        if(hud != null) hud.gameObject.SetActive(true);
    }
    public void ShowPlaceable()
    {

        

        Place curPlace = selectedPiece.place;
        Vector2Int curIndex = curPlace.boardIndex;
        Board curBoard = curPlace.board;
       
        if (null == curBoard)                   // 기물이 있는 곳이 보드가 아니라면 종료
            return;

        if (!curBoard.FollowRule)               // 규칙을 따르지 않는 보드라면 종료
            return;

        //selectedPiece.IsMovable(selectedPiece.place.boardIndex);


    }

    public void PostPlaceAction()
    {
        Place newPlace = selectedPiece.place;
        Vector2Int newIndex = newPlace.boardIndex;
        Board curBoard = newPlace.board;

        // 기물이 있는 곳이 보드가 아니라면 종료
        if (null == curBoard)
            return;

        // 규칙을 따르지 않는 보드라면 종료
        if (!curBoard.FollowRule)
            return;

        selectedPiece.IsMovable(selectedPiece.place.boardIndex);

       
    }

    



    public void ShowPlaceableEnd(Place oldPlace)
    {



        //List<Vector2Int> locationList = selectedPiece.MovableTo;


      


    }

    public void ChangePlaceColor(Vector2Int location, PlaceType placeType)
    {
        switch(placeType)
        {
            case PlaceType.DEFENCE:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(Color.blue);
                break;

            case PlaceType.ATTACK:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(Color.red);
                break;

            case PlaceType.NORMAL:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor();
                break;

            case PlaceType.MOVABLE:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(highlight);
                break;

            case PlaceType.SPECIALMOVE:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(Color.gray);
                break;
        }
    }

    public void MovePieceTo(Place place)
    {
        Place oldPlace = selectedPiece.place;
        oldPlace.piece = null;

        ShowPlaceableEnd(oldPlace);

        selectedPiece.SetInPlace(place);    // 기물이 밟는 위치 변경됨
        place.piece = selectedPiece;
        PostPlaceAction();


        SelectedPieceInit(oldPlace);
    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        ShowPlaceable();
        GameManager.Instance.state = GameManager.GameState.SELECTING_PLACE;

        camController.SetFreeCam(selectedPiece.transform);

    }
    public void SelectedPieceInit(Place oldPlace)
    {
        
        SelectedPiece = null;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;

        camController.ChangeFreeCamPriority(10);
        camController.ChangeVCamPriority(20);
    }
}
