using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecidedStateLists : StateLists, IChessEventable
{
    public void Attack()
    {
        Debug.Log("���� �̺�Ʈ");
    }

    public void Check(Piece targetPiece)
    {
        Debug.Log("üũ �̺�Ʈ");
        ChessEventManager.Instance.CheckEvent(targetPiece);
    }

    public void CheckMate()
    {
        Debug.Log("üũ ����Ʈ �̺�Ʈ");
    }

    public void Defend()
    {
        Debug.Log("��� �̺�Ʈ");
        // ������ ����ϴ� �⹰�� ���� �� �����ϴ°�?

    }

    public void Threat()
    {
        Debug.Log("���� �̺�Ʈ");
    }
}
