using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReturnHeat
{
    int ReturnTeamHeat(Place place);

    int ReturnOpponentHeat(Place place);
}
