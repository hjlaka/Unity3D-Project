using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessEventManager : SingleTon<ChessEventManager>
{
    public enum EventType { CHECK, CHECKMATE, GAME_END }

    public UnityEvent OnAIChecked;

    public void AddEvent(EventType eventType, ITalkable talkable)
    {
        // 주체가 되는 기물
        
    }

    private void FindEvent()
    {
        // 이벤트를 차례대로 찾는다. (우선순위 높은 순서대로)
    }

    public void IsCheckMate()
    {

    }

    public void CheckEvent(Piece piece)
    {


        piece.ChangeColor(Color.red);
        switch(piece.team.direction)
        {
            case TeamData.Direction.DownToUp:
                Debug.Log("플레이어의 왕이 위기다.");
                break;

            case TeamData.Direction.UpToDown:
                // 팀 정보에 따라 다른 동작 호출
                Debug.Log("AI의 왕이 위기다.");
                // AI의 반응 호출
                OnAIChecked?.Invoke();                
                break;
        }
    }


}
