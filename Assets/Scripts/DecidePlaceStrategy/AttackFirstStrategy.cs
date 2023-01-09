using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFirstStrategy : MonoBehaviour, IDecidePlaceStrategy
{
    public void DecidePlace()
    {
        Debug.Log("공격 우선 선택");
    }
}
