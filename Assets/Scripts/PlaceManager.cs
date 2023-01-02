using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : SingleTon<PlaceManager>
{
    public Piece selectedPiece;

    public Place selectedPlace;


    [SerializeField]
    private Color highlight;


    public void ShowPlaceable()
    {


        Place curPlace = selectedPiece.place;
        Vector2Int curIndex = curPlace.boardIndex;
        Board curBoard = curPlace.board;

        // 기물이 있는 곳이 보드가 아니라면 종료
        if (null == curBoard)
            return;

        for(int i = 0; i < curBoard.places.GetLength(0); i++)
        {
            for (int j = 0; j < curBoard.places.GetLength(1); j++)
            {
                // 좌표에 기물이 있다면
                if (curBoard.places[i, j].piece != null)
                {
                    // 아군 기물이라면 건너 뛴다.
                    continue;

                    // 적군 기물이라면 잡을 수 있다고 표시한다.
                }

                Vector2Int index = new Vector2Int(i, j);
                // 이동할 수 있는 영역인지 계산
                if (selectedPiece.IsMovable(index))
                {
                    // 이동할 수 있는 영역이라면
                    curBoard.places[i, j].ChangeColor(highlight);
                    Debug.Log("이동 가능!: " + curBoard.places[i, j].gameObject.name);
                }
                else
                {
                    Debug.Log("이동 불가능!: " + curBoard.places[i, j].gameObject.name);
                    // 이동할 수 있는 영역이 아니라면
                }

            }

        }
    }

    public void ShowPlaceableEnd(Place oldPlace)
    {
        
        Board oldBoard = oldPlace.board;

        if (null == oldBoard)
            return;


        for (int i = 0; i < oldBoard.places.GetLength(0); i++)
        {
            for (int j = 0; j < oldBoard.places.GetLength(1); j++)
            {
                oldBoard.places[i, j].ChangeColor();
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
        ShowPlaceableEnd(oldPlace);
    }

    private void ControlInit()
    {
        selectedPiece = null;
       
    }
}
