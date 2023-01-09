using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendFirstStategy : MonoBehaviour, IDecidePlaceStrategy
{
    public void DecidePlace()
    {
        Debug.Log("방어 먼저 전략");
    }
}
