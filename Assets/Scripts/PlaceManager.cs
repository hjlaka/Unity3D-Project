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

        // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
        if (null == curBoard)
            return;

        // ��Ģ�� ������ �ʴ� ������ ����
        if (!curBoard.FollowRule)
            return;

        for(int i = 0; i < curBoard.places.GetLength(0); i++)
        {
            for (int j = 0; j < curBoard.places.GetLength(1); j++)
            {


                Vector2Int index = curBoard.places[i, j].boardIndex;
                // �̵��� �� �ִ� �������� ���
                if (selectedPiece.IsMovable(index))
                {
                    // �̵��� �� �ִ� �����̶��
                    Piece account = curBoard.places[i, j].piece;
                    // ��ǥ�� �⹰�� �ִٸ�
                    if (account != null)
                    {
                        // �Ʊ� �⹰�̶��
                        if (account.team.TeamId == selectedPiece.team.TeamId)
                        {
                            selectedPiece.AddDefence(account);
                            continue;
                        }
                        // ���� �⹰�̶��
                        else
                        {
                            selectedPiece.AddAttack(account);
                            curBoard.places[i, j].ChangeColor(attackable);
                        }
                    }
                    else
                    {
                        curBoard.places[i, j].ChangeColor(highlight);
                        //Debug.Log("�̵� ����!: " + curBoard.places[i, j].gameObject.name);
                        curBoard.places[i, j].IsApprochable = true;
                    }
                }
                else
                {
                    //Debug.Log("�̵� �Ұ���!: " + curBoard.places[i, j].gameObject.name);
                    // �̵��� �� �ִ� ������ �ƴ϶��
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
        // �Ҹ��� ���� �Ѵٸ�
        // OnExitSelect?.Invoke();
    }
}
