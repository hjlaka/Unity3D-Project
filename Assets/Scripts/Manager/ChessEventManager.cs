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
        // ��ü�� �Ǵ� �⹰
        
    }

    private void FindEvent()
    {
        // �̺�Ʈ�� ���ʴ�� ã�´�. (�켱���� ���� �������)
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
                Debug.Log("�÷��̾��� ���� �����.");
                break;

            case TeamData.Direction.UpToDown:
                // �� ������ ���� �ٸ� ���� ȣ��
                Debug.Log("AI�� ���� �����.");
                // AI�� ���� ȣ��
                OnAIChecked?.Invoke();                
                break;
        }
    }


}
