using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnBoardTargetable
{
    enum Type { Peace, Attack /*, Gain*/}
    Type React();
    Vector3 GetPosition();

    Place GetPlace();
    //Unit GetUnit();
}
