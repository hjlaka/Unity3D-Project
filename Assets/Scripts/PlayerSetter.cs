using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetter : MonoBehaviour
{
    [SerializeField]
    private Board mainBoard;

    [SerializeField]
    private Vector2Int placeableMin;
    [SerializeField]
    private Vector2Int placeableMax;
    public void GetBoard()
    {
        mainBoard = GameObject.Find("MainBoard").GetComponent<Board>();

        if (mainBoard == null)
            Debug.LogError("보드를 찾지 못함");
    }
    public void MakeBoardSetable()
    {
        // Place의 변수를 placeable로 바꾼다. (혹은 Movable)
        // 보드를 가져온다.

        for(int i = placeableMin.x; i < placeableMax.x; i++)
        {
            for(int j = placeableMin.y; j < placeableMax.y; j++)
            {
                if (mainBoard.places.GetLength(0) < i)
                {
                    Debug.LogError("x 범위 초과");
                    return;
                }
                if (mainBoard.places.GetLength(1) < j)
                {
                    Debug.LogError("y 범위 초과");
                    return;
                }

                mainBoard.places[i, j].IsMovableToCurPiece = true;
                mainBoard.places[i, j].ChangeColor(Color.yellow);
            }
        }
    }

    public void MakeBoardSetableNot()
    {
        for (int i = placeableMin.x; i < placeableMax.x; i++)
        {
            for (int j = placeableMin.y; j < placeableMax.y; j++)
            {
                if (mainBoard.places.GetLength(0) < i)
                {
                    Debug.LogError("x 범위 초과");
                    return;
                }
                if (mainBoard.places.GetLength(1) < j)
                {
                    Debug.LogError("y 범위 초과");
                    return;
                }

                mainBoard.places[i, j].IsMovableToCurPiece = false;
                mainBoard.places[i, j].ChangeColor();
            }
        }
    }


}
