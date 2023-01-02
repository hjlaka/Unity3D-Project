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

        // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
        if (null == curBoard)
            return;

        for(int i = 0; i < curBoard.places.GetLength(0); i++)
        {
            for (int j = 0; j < curBoard.places.GetLength(1); j++)
            {
                // ��ǥ�� �⹰�� �ִٸ�
                if (curBoard.places[i, j].piece != null)
                {
                    // �Ʊ� �⹰�̶�� �ǳ� �ڴ�.
                    continue;

                    // ���� �⹰�̶�� ���� �� �ִٰ� ǥ���Ѵ�.
                }

                Vector2Int index = new Vector2Int(i, j);
                // �̵��� �� �ִ� �������� ���
                if (selectedPiece.IsMovable(index))
                {
                    // �̵��� �� �ִ� �����̶��
                    curBoard.places[i, j].ChangeColor(highlight);
                    Debug.Log("�̵� ����!: " + curBoard.places[i, j].gameObject.name);
                }
                else
                {
                    Debug.Log("�̵� �Ұ���!: " + curBoard.places[i, j].gameObject.name);
                    // �̵��� �� �ִ� ������ �ƴ϶��
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
