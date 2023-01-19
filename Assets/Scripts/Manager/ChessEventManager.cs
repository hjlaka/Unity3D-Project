using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessEventManager : SingleTon<ChessEventManager>
{
   


    private List<ChessEvent> eventList;

    private Dictionary<string, ChessEvent> relationDictionary;

    public UnityEvent OnAIChecked;

    private void Awake()
    {
        eventList = new List<ChessEvent>();
        relationDictionary = new Dictionary<string, ChessEvent>();
    }

    private void AddEvent(ChessEvent chessEvent)
    {
        // ��ü�� �Ǵ� �⹰
        eventList.Add(chessEvent);
        Debug.Log("----------------- �̺�Ʈ �߰� ------------------- : " + eventList.Count);
        
    }

    public void SubmitEvent(ChessEvent chessEvent)
    {
        Piece subject = chessEvent.Subject;
        Piece target = chessEvent.Subject;
        string key = subject.GetInstanceID() + "/" + subject.GetInstanceID();
        bool relationExist = relationDictionary.ContainsKey(key);
        if(relationExist)
        {
            if(chessEvent.GetType() == relationDictionary[key].GetType())
            {
                Debug.Log("�̹� �ִ� �̺�Ʈ");
            }
            else
            {
                Debug.Log("�̹� �ִ� �̺�Ʈ�� �ٸ�");
            }
        }
        else
        {
            Debug.Log("���ο� �̺�Ʈ " + chessEvent.GetTypeAsString());
            AddEvent(chessEvent);
        }

    }

    public void GetEvent()
    {
        if (eventList.Count <= 0) return;

        string pieceName;
        string talk;

        Debug.Log("�̺�Ʈ ����: " + eventList.Count);
        // ��� �̺�Ʈ �� �ֱ�
        for (int i = 0; i < eventList.Count; i++)
        {
            ChessEvent chessEvent = eventList[i];
            Piece subject = chessEvent.Subject;
            talk = chessEvent.GetTypeAsString();
            
            DialogueManager.DialogueUnit dialogue = new DialogueManager.DialogueUnit(subject, talk);
            // ĳ���� ��� �������� 

            DialogueManager.Instance.AddDialogue(dialogue);

            if (chessEvent.Target != null)
            {
                subject = chessEvent.Target;
                talk = chessEvent.GetTypeAsString() + "response";
                DialogueManager.DialogueUnit dialogue2 = new DialogueManager.DialogueUnit(subject, talk);
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
