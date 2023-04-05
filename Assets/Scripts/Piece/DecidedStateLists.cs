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


    public void Attack(Piece subject, Piece target)
    {
        Debug.Log("공격 이벤트");
    }

    public void Check(Piece subject, Piece target)
    {
        Debug.Log("체크 이벤트");
        //ChessEventManager.Instance.CheckEvent(target);
        ChessEvent chessEvent = new ChessEvent(ChessEvent.EventType.CHECK, subject, target);
        ChessEventManager.Instance.SubmitEvent(chessEvent);
    }

    public void CheckMate(Piece subject, Piece target)
    {
        Debug.Log("체크 메이트 이벤트");
    }

    public void Defend(Piece subject, Piece target)
    {

        Place targetPlace = target.place;
        int deltaTopBottomHeat = targetPlace.HeatPointTopTeam - targetPlace.HeatPointBottomTeam;
        //Debug.Log(string.Format("위팀 과열도 {0} - 아래팀 과열도 {1} = {2}", targetPlace.HeatPointTopTeam, targetPlace.HeatPointBottomTeam, deltaTopBottomHeat));
        
        // 예외처리
        if (target.team.direction == TeamData.Direction.UpToDown)
        {
            if (targetPlace.HeatPointTopTeam > targetPlace.HeatPointBottomTeam)
                //위팀 과열도가 더 높은 상태라면
                return;
        }
        else
        {
            if (targetPlace.HeatPointTopTeam < targetPlace.HeatPointBottomTeam)
                //아래팀 과열도가 더 높은 상태라면
                return;
        }


        Debug.Log("방어 이벤트: " + subject + " " + target + " 과열도 차이: " + deltaTopBottomHeat);
        // 이전에 방어하던 기물에 대해 방어를 유지하는가?

        // 방어 이벤트 제출
        ChessEvent chessEvent = new ChessEvent(ChessEvent.EventType.DEFENCE, subject, target);
        ChessEventManager.Instance.SubmitEvent(chessEvent);


    }

    public void Threat(Piece subject, Piece target)
    {
        Debug.Log("위협 이벤트");
        // 위협 이벤트 제출
        ChessEvent chessEvent = new ChessEvent(ChessEvent.EventType.THREAT, subject, target);
        ChessEventManager.Instance.SubmitEvent(chessEvent);
    }
}
