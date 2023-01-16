using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecidedStateLists : StateLists, IChessEventable
{
    public void Attack()
    {
        Debug.Log("공격 이벤트");
    }

    public void Check()
    {
        Debug.Log("체크 이벤트");
    }

    public void CheckMate()
    {
        Debug.Log("체크 메이트 이벤트");
    }

    public void Defend()
    {
        Debug.Log("방어 이벤트");
    }

    public void Threat()
    {
        Debug.Log("위협 이벤트");
    }
}
