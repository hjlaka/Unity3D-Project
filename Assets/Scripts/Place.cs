using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    public Piece piece;

    private void OnMouseDown()
    {
        //Debug.Log(string.Format("{0} Ŭ��", gameObject.name));

        // �⹰�� ���õ� ���¿��� Ŭ����

        if (null == PlaceManager.Instance.selectedPiece)
            return;

        if (piece != null)
            return;

        PlaceManager.Instance.MovePieceTo(this);


    }


    //{
    // �⹰ ���� ��Ȳ����

    // �⹰�� �ִ� ���
    // �⹰ ����
    //}

}
