using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaftyPreference : MonoBehaviour, IPlacePreference
{
    public float ReturnHeatPreference(Piece piece, Place place)
    {
        //안정 우선:

        // 상대팀 위협이 하나라도 있으면 우선도가 떨어지는 경우
        // 상대팀 위협이 있어도, 중요도가 높다면(?), 팀을 믿고 이동하는 경우

        // 먹을 수 있는 기물의 경우에는... 그 기물의 과열도를 하나 빼야 한다.
        int deltaHeat = piece.returnHeat.ReturnTeamHeat(place) - piece.returnHeat.ReturnOpponentHeat(place) - 1;  // 자기 자신의 과열도 삐기
        float heatPreferPoint = 0;
        if (0 == deltaHeat)
        {
            heatPreferPoint = 3;
        }
        else if (deltaHeat == 1)
        {
            heatPreferPoint = 4;
        }
        else if (deltaHeat == -1)
        {
            heatPreferPoint = 1;
        }
        else if (deltaHeat > 1)
        {
            heatPreferPoint = 4;
        }
        else if (deltaHeat < -1)
        {
            heatPreferPoint = 0;
        }

        if (piece.returnHeat.ReturnOpponentHeat(place) > 0)
        {
            heatPreferPoint *= 0.5f;
        }

        return heatPreferPoint;
    }
}
