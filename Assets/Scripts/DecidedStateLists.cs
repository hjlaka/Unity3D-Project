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
        // 보호 이벤트 발동
    }

    public override void AddThreating(Piece piece)
    {
        threating.Add(piece);
        // 위협 이벤트 발동
    }


    public void Attack()
    {
        Debug.Log("공격 이벤트");
    }

    public void Check(Piece targetPiece)
    {
        Debug.Log("체크 이벤트");
        ChessEventManager.Instance.CheckEvent(targetPiece);
    }

    public void CheckMate()
    {
        Debug.Log("체크 메이트 이벤트");
    }

    public void Defend()
    {
        Debug.Log("방어 이벤트");
        // 이전에 방어하던 기물에 대해 방어를 유지하는가?

    }

    public void Threat()
    {
        Debug.Log("위협 이벤트");
    }
}
