using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendFirstStategy : IDecidePlaceStrategy
{
    public Place DecidePlace(Piece piece)
    {
        Debug.Log("방어 먼저 전략");

        Place place = null;


        // 일단 이동해보고 방어가 가능한지 안 가능한지 따져야 한다.

        // 혹은 방어를 유지한 채 이동하기?

        // 혹은 그냥 회피 이동 우선?


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
