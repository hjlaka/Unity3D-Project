using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecidedStateLists : StateLists, IChessEventable
{
    public void Attack()
    {
        Debug.Log("���� �̺�Ʈ");
    }

    public void Check()
    {
        Debug.Log("üũ �̺�Ʈ");
    }

    public void CheckMate()
    {
        Debug.Log("üũ ����Ʈ �̺�Ʈ");
    }

    public void Defend()
    {
        Debug.Log("��� �̺�Ʈ");
    }

    public void Threat()
    {
        Debug.Log("���� �̺�Ʈ");
    }
}
