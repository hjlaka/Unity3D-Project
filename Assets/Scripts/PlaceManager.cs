using Cinemachine;
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

                if (selectedPiece.place.boardIndex == index)
                    continue;
                //현재 위치

                else if (selectedPiece.IsMovable(index))
                {
                    // 이동할 수 있는 영역이라면
                    Piece account = curBoard.places[i, j].piece;               
                    // 좌표에 기물이 있다면
                    if (account != null)
                    {
                        // 아군 기물이라면
                        if (account.team.TeamId == selectedPiece.team.TeamId)
                        {
                            continue;
                        }
                        // 적군 기물이라면
                        else
                        {
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

        for (int i = 0; i < curBoard.places.GetLength(0); i++)
        {
            for (int j = 0; j < curBoard.places.GetLength(1); j++)
            {
                Vector2Int index = curBoard.places[i, j].boardIndex;
                // 이동할 수 있는 영역인지 계산

                //현재 위치
                if (selectedPiece.place.boardIndex == index)
                    continue;

                else if (selectedPiece.IsMovable(index))
                {
                    // 이동할 수 있는 영역이라면
                    Place cell = curBoard.places[i, j];
                    Piece account = cell.piece;
                    
                    cell.HeatPoint++;

                    // 좌표에 기물이 있다면
                    if (account != null)
                    {
                        // 아군 기물이라면
                        if (account.team.TeamId == selectedPiece.team.TeamId)
                        {
                            selectedPiece.AddDefence(account);
                            account.BeDefended(selectedPiece);
                            //curBoard.places[i, j].ChangeColor(defended);
                        }
                        // 적군 기물이라면
                        else
                        {
                            selectedPiece.AddAttack(account);
                            account.BeAttacked(selectedPiece);
                            //curBoard.places[i, j].ChangeColor(attackable);
                        }
                    }
                    else
                    {
                        //curBoard.places[i, j].ChangeColor(highlight);
                        //curBoard.places[i, j].IsApprochable = true;

                        // 현재 아무것도 없는 칸도 보호받을 수 있다.
                    }
                }
            }
        }
        curBoard.UpdateHeatHUD();
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
    }
    public void SelectedPieceInit(Place oldPlace)
    {
        ShowPlaceableEnd(oldPlace);
        SelectedPiece = null;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;
        // 소리도 나야 한다면
        // OnExitSelect?.Invoke();
    }
}
