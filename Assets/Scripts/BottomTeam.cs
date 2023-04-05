using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomTeam : IReturnHeat
{
    public int ReturnTeamHeat(Place place)
    {
        return place.HeatPointBottomTeam;
    }
    public int ReturnOpponentHeat(Place place)
    {
        return place.HeatPointTopTeam;
    }

}
