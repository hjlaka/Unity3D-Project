using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendFirstStategy : IDecidePlaceStrategy
{
    public Place DecidePlace(Piece piece)
    {
        Debug.Log("��� ���� ����");

        Place place = null;


        // �ϴ� �̵��غ��� �� �������� �� �������� ������ �Ѵ�.

        // Ȥ�� �� ������ ä �̵��ϱ�?

        // Ȥ�� �׳� ȸ�� �̵� �켱?


        /*int defencePoint = 0;

        for (int i = 0; i < movablePlaces.Count; i++)
        {
            if(movablePlaces[i].HeatPoint >= defencePoint)
            {
                place = movablePlaces[i];
                defencePoint = movablePlaces[i].HeatPoint;
            }    
        }
*/
        return place;
        
    }
}
