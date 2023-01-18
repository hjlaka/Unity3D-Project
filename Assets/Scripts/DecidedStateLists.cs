using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DecidedStateLists : StateLists, IChessEventable
{

    public override void AddMovable(Place place)
    {
        movable.Add(place);
    }
    public override void AddDefending(Piece piece)
    {
        defending.Add(piece);
        // ��ȣ �̺�Ʈ �ߵ�
    }

    public override void AddThreating(Piece piece)
    {
        threating.Add(piece);
        // ���� �̺�Ʈ �ߵ�
    }


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

    public void Defend(Piece subject, Piece target)
    {

        Place targetPlace = target.place;
        int deltaTopBottomHeat = targetPlace.HeatPointTopTeam - targetPlace.HeatPointBottomTeam;
        //Debug.Log(string.Format("���� ������ {0} - �Ʒ��� ������ {1} = {2}", targetPlace.HeatPointTopTeam, targetPlace.HeatPointBottomTeam, deltaTopBottomHeat));
        
        // ����ó���� �ʿ��ϴ�.

        if (target.team.direction == TeamData.Direction.UpToDown)
        {
            if (deltaTopBottomHeat > 0)
                return;
        }
        else
        {
            if (deltaTopBottomHeat <= 0)
                return;
        }


        Debug.Log("��� �̺�Ʈ: " + subject + " " + target + " ������ ����: " + deltaTopBottomHeat);
        // ������ ����ϴ� �⹰�� ���� �� �����ϴ°�?

    }

    public void Threat()
    {
        Debug.Log("���� �̺�Ʈ");
    }
}
