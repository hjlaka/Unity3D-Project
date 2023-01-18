using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessEventManager : SingleTon<ChessEventManager>
{
   


    private List<ChessEvent> eventList;

    public UnityEvent OnAIChecked;

    private void Awake()
    {
        eventList = new List<ChessEvent>();
    }

    public void AddEvent(ChessEvent chessEvent)
    {
        // ��ü�� �Ǵ� �⹰
        eventList.Add(chessEvent);
        
    }

    public void GetEvent()
    {
        if (eventList.Count <= 0) return;

        // ��� �̺�Ʈ �� �ֱ�
        for(int i = 0; i < eventList.Count; i++)
        {
            ChessEvent chessEvent = eventList[i];
            DialogueManager.DialogueUnit dialogue = new DialogueManager.DialogueUnit(chessEvent.Subject.character.name, "�̺�Ʈ 1");
            // ĳ���� ��� ��������

            DialogueManager.Instance.AddDialogue(dialogue);

            if(chessEvent.Target != null)
            {
                DialogueManager.DialogueUnit dialogue2 = new DialogueManager.DialogueUnit(chessEvent.Target.character.name, "�̺�Ʈ 2");
                DialogueManager.Instance.AddDialogue(dialogue2);
                // ĳ���� ���� ��������
            }
        }

        eventList.Clear();
        
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
