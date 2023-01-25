using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessEventManager : SingleTon<ChessEventManager>
{
   


    private Heap<ChessEvent> eventList;

    private Dictionary<string, ChessEvent> relationDictionary;

    public UnityEvent OnAIChecked;

    private void Awake()
    {
        eventList = new Heap<ChessEvent>(new Greater());
        relationDictionary = new Dictionary<string, ChessEvent>();
    }

    private void AddEvent(float importance, ChessEvent chessEvent)
    {
        // ��ü�� �Ǵ� �⹰
        Node<ChessEvent> node = new Node<ChessEvent>(importance, chessEvent);
        eventList.Push(node);
        Debug.Log("----------------- �̺�Ʈ �߰� ------------------- : " + eventList.Count);

        eventList.PrintHeap();

    }

    public void SubmitEvent(ChessEvent chessEvent)
    {
        switch(chessEvent.Type)
        {
            case ChessEvent.EventType.GAME_END:
                Debug.Log("���� ���� �̺�Ʈ ����");
                AddEvent(9999, chessEvent);
                break;
            default:
                Piece subject = chessEvent.Subject;
                Piece target = chessEvent.Target;
                string key = subject.GetInstanceID() + "/" + target.GetInstanceID();
                bool relationExist = relationDictionary.ContainsKey(key);
                if (relationExist)
                {
                    if (chessEvent.GetType() == relationDictionary[key].GetType())
                    {
                        Debug.Log("�̹� �ִ� �̺�Ʈ--------------------------------");
                    }
                    else
                    {
                        Debug.Log("�̹� �ִ� �̺�Ʈ�� �ٸ�");
                    }
                }
                else
                {
                    Debug.Log("���ο� �̺�Ʈ " + subject + " " + target + " " + chessEvent.GetTypeAsString());
                    Debug.Log("Ű: [" + key + "]");
                    relationDictionary.Add(key, chessEvent);
                    float importance = subject.PieceScore + target.PieceScore + target.place.HeatPoint;
                    AddEvent(importance, chessEvent);
                }
                break;
        }
    }

    public void GetEvent()
    {
        if (eventList.Count <= 0) return;

        string pieceName;
        string talk;

        Debug.Log("�̺�Ʈ ����: " + eventList.Count);
        Debug.Log("ù��° �̺�Ʈ �߿䵵: " + eventList.GetNode(0).value);
        Debug.Log("������ �̺�Ʈ �߿䵵: " + eventList.GetNode(eventList.Count - 1).value);

        // �߿��� �̺�Ʈ�� �ߵ���Ű��
        Node<ChessEvent> node = eventList.Pop();
        ChessEvent chessEvent = node.obj;
        Debug.Log("�̺�Ʈ �߿䵵: " + node.value);

        if (chessEvent.Type == ChessEvent.EventType.GAME_END)
        {
            DialogueManager.DialogueUnit endDialouge = new DialogueManager.DialogueUnit("��", "���� ���� ����");
            DialogueManager.Instance.AddDialogue(endDialouge);

        }
        else
        {
            Piece subject = chessEvent.Subject;
            talk = GetDialogue(subject.character, chessEvent.Type);
            DialogueManager.DialogueUnit dialogue = new DialogueManager.DialogueUnit(subject, talk);
            // ĳ���� ��� �������� 

            DialogueManager.Instance.AddDialogue(dialogue);

            if (chessEvent.Target != null)
            {
                subject = chessEvent.Target;
                talk = GetResponseDialogue(subject.character, chessEvent.Type);
                DialogueManager.DialogueUnit dialogue2 = new DialogueManager.DialogueUnit(subject, talk);
                DialogueManager.Instance.AddDialogue(dialogue2);
                // ĳ���� ���� ��������
            }
        }

       
        

        // ������ ��ϵ� �̺�Ʈ ��ȸ.
        // �����ڷ� ��� ��Ű��.
        // Ȥ�� �ļ� ���� ��� ��Ű��.

       

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

    public string GetDialogue(CharacterData character, ChessEvent.EventType eventType)
    {
        switch(eventType)
        {
            case ChessEvent.EventType.ATTACK:
                return character.attacking;
            case ChessEvent.EventType.THREAT:
                return character.threating;
            case ChessEvent.EventType.DEFENCE:
                return character.defending;
            case ChessEvent.EventType.RETURN:
                return "Return";
            case ChessEvent.EventType.CHECK:
                return "Check";
            default:
                return "Default";
        }
    }

    public string GetResponseDialogue(CharacterData character, ChessEvent.EventType eventType)
    {
        switch (eventType)
        {
            case ChessEvent.EventType.ATTACK:
                return character.beAttacked;
            case ChessEvent.EventType.THREAT:
                return character.beThreated;
            case ChessEvent.EventType.DEFENCE:
                return character.beDefended;
            case ChessEvent.EventType.RETURN:
                return "Return";
            case ChessEvent.EventType.CHECK:
                return "Check";
            default:
                return "Default";
        }
    }


}
