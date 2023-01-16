using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessEventManager : SingleTon<ChessEventManager>
{
    public UnityEvent OnAIChecked;
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
