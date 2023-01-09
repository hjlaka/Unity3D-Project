using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTeam : IReturnHeat
{
    public int ReturnTeamHeat(Place place)
    {
        return place.HeatPointTopTeam;
    }
    public int ReturnOpponentHeat(Place place)
    {
        return place.HeatPointBottomTeam;
    }


}
