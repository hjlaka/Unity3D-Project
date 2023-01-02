using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceManager : SingleTon<PlaceManager>
{
    [SerializeField]
    private Piece selectedPiece;
    public Piece SelectedPiece
    {
        get { return selectedPiece; }
        set 
        { 
            selectedPiece = value;
        }
    }

    public UnityEvent OnExitSelect;

    public Place selectedPlace;


    [SerializeField]
    private Color highlight;
    [SerializeField]
    private Color attackable;


    public void ShowPlaceable()
    {
        

        Place curPlace = selectedPiece.place;
        Vector2Int curIndex = curPlace.boardIndex;
        Board curBoard = curPlace.board;

        // 기물이 있는 곳이 보드가 아니라면 종료
        if (null == curBoard)
            return;

        // 규칙을 따르지 않는 보드라면 종료
        if (!curBoard.FollowRule)
            return;

        for(int i = 0; i < curBoard.places.GetLength(0); i++)
        {
            for (int j = 0; j < curBoard.places.GetLength(1); j++)
            {


                Vector2Int index = curBoard.places[i, j].boardIndex;
                // 이동할 수 있는 영역인지 계산
                if (selectedPiece.IsMovable(index))
                {
                    // 이동할 수 있는 영역이라면
                    Piece account = curBoard.places[i, j].piece;
                    // 좌표에 기물이 있다면
                    if (account != null)
                    {
                        // 아군 기물이라면
                        if (account.team.TeamId == selectedPiece.team.TeamId)
                        {
                            selectedPiece.AddDefence(account);
                            continue;
                        }
                        // 적군 기물이라면
                        else
                        {
                            selectedPiece.AddAttack(account);
                            curBoard.places[i, j].ChangeColor(attackable);
                        }
                    }
                    else
                    {
                        curBoard.places[i, j].ChangeColor(highlight);
                        //Debug.Log("이동 가능!: " + curBoard.places[i, j].gameObject.name);
                        curBoard.places[i, j].IsApprochable = true;
                    }
                }
                else
                {
                    //Debug.Log("이동 불가능!: " + curBoard.places[i, j].gameObject.name);
                    // 이동할 수 있는 영역이 아니라면
                    curBoard.places[i, j].IsApprochable = false;
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
                oldBoard.places[i, j].IsApprochable = true;
            }

        }
    }

    public void MovePieceTo(Place place)
    {
        Place oldPlace = selectedPiece.place;
        oldPlace.piece = null;

        selectedPiece.SetInPlace(place);
        place.piece = selectedPiece;


        SelectedPieceInit();
    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        ShowPlaceable();
        GameManager.Instance.state = GameManager.GameState.SELECTING_PLACE;
    }
    public void SelectedPieceInit()
    {
        ShowPlaceableEnd(selectedPiece.place);
        SelectedPiece = null;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;
        // 소리도 나야 한다면
        // OnExitSelect?.Invoke();
    }
}
