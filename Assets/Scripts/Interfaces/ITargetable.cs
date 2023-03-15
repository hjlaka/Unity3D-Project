using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    enum Type { Peace, Attack /*, Gain*/}
    Type React();
    Vector3 GetPosition();

    //Place GetPlace();
    //Unit GetUnit();
}
