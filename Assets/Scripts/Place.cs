using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    public Piece piece;

    private void OnMouseDown()
    {
        //Debug.Log(string.Format("{0} 클릭", gameObject.name));

        // 기물이 선택된 상태에서 클릭시

        if (null == PlaceManager.Instance.selectedPiece)
            return;

        if (piece != null)
            return;

        PlaceManager.Instance.MovePieceTo(this);


    }


    //{
    // 기물 선택 상황에서

    // 기물이 있는 경우
    // 기물 선택
    //}

}
