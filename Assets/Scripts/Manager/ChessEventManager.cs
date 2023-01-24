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
        // 주체가 되는 기물
        eventList.Add(chessEvent);
        Debug.Log("----------------- 이벤트 추가 ------------------- : " + eventList.Count);
        
    }

    public void SubmitEvent(ChessEvent chessEvent)
    {
        Piece subject = chessEvent.Subject;
        Piece target = chessEvent.Target;
        string key = subject.GetInstanceID() + "/" + target.GetInstanceID();
        bool relationExist = relationDictionary.ContainsKey(key);
        if(relationExist)
        {
            if(chessEvent.GetType() == relationDictionary[key].GetType())
            {
                Debug.Log("이미 있는 이벤트--------------------------------");
            }
            else
            {
                Debug.Log("이미 있는 이벤트와 다름");
            }
        }
        else
        {
            Debug.Log("새로운 이벤트 " + subject + " " + target + " " + chessEvent.GetTypeAsString());
            Debug.Log("키: [" + key + "]");
            relationDictionary.Add(key, chessEvent);
            AddEvent(chessEvent);
        }

    }

    public void GetEvent()
    {
        if (eventList.Count <= 0) return;

        string pieceName;
        string talk;

        Debug.Log("이벤트 개수: " + eventList.Count);
        // 모든 이벤트 다 넣기
        for (int i = 0; i < eventList.Count; i++)
        {
            ChessEvent chessEvent = eventList[i];
            Piece subject = chessEvent.Subject;
            talk = GetDialogue(subject.character, chessEvent.Type);
            DialogueManager.DialogueUnit dialogue = new DialogueManager.DialogueUnit(subject, talk);
            // 캐릭터 대사 가져오기 

            DialogueManager.Instance.AddDialogue(dialogue);

            if (chessEvent.Target != null)
            {
                subject = chessEvent.Target;
                talk = GetResponseDialogue(subject.character, chessEvent.Type);
                DialogueManager.DialogueUnit dialogue2 = new DialogueManager.DialogueUnit(subject, talk);
                DialogueManager.Instance.AddDialogue(dialogue2);
                // 캐릭터 반응 가져오기
            }
        }

        eventList.Clear();
        
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
