using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessEventManager : SingleTon<ChessEventManager>
{
    private Heap<ChessEvent> eventList;
    //========디버그용===========
    [SerializeField]
    private List<ChessEvent> eventsDebug;
    //==========================

    private Dictionary<string, ChessEvent> relationDictionary;

    public UnityEvent OnAIChecked;

    private void Awake()
    {
        eventList = new Heap<ChessEvent>(new Greater());
        relationDictionary = new Dictionary<string, ChessEvent>();

        eventsDebug = new List<ChessEvent>();
    }

    private void AddEvent(float importance, ChessEvent chessEvent)
    {
        // 주체가 되는 기물
        Node<ChessEvent> node = new Node<ChessEvent>(importance, chessEvent);
        eventList.Push(node);
        eventsDebug.Add(chessEvent);
        Debug.Log(string.Format("----------------- 이벤트 추가 ------------------- : {0} ({1}가 {2}에게 이벤트 발동)", 
            eventList.Count, chessEvent.Subject, chessEvent.Target));

        eventList.PrintHeap();

    }

    public void SubmitEvent(ChessEvent chessEvent)
    {
        Debug.Log(string.Format("게임 이벤트 제출: {0}", chessEvent));
        switch (chessEvent.Type)
        {
            case ChessEvent.EventType.GAME_END:
                Debug.Log("게임 종료 이벤트 제출");
                AddEvent(chessEvent.GetEventTypeScore(), chessEvent);
                break;
            default:
                Piece subject = chessEvent.Subject;
                Piece target = chessEvent.Target;
                string key = subject.GetInstanceID() + "/" + target.GetInstanceID();
                Debug.Log(string.Format("키: {0}", key));
                bool relationExist = relationDictionary.ContainsKey(key);
                if (relationExist)
                {
                    if (chessEvent.Type == relationDictionary[key].Type)
                    {
                        Debug.Log("이미 있는 이벤트--------------------------------" + chessEvent.Type);
                    }
                    else
                    {
                        Debug.Log("이미 있는 이벤트와 다름");
                        relationDictionary[key] = chessEvent;
                        float importance = subject.PieceScore + target.PieceScore + target.place.HeatPoint + chessEvent.GetEventTypeScore();
                        AddEvent(importance, chessEvent);

                    }
                }
                else
                {
                    Debug.Log("새로운 이벤트 " + subject + " " + target + " " + chessEvent.ToString());
                    Debug.Log("키: [" + key + "]");
                    relationDictionary.Add(key, chessEvent);
                    float importance = subject.PieceScore + target.PieceScore + target.place.HeatPoint + chessEvent.GetEventTypeScore();
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


        // 중요한 이벤트만 발동시키기
        Node<ChessEvent> node = eventList.Pop();
        ChessEvent chessEvent = node.obj;
        Debug.Log("이벤트 중요도: " + node.value);

        if (chessEvent.Type == ChessEvent.EventType.GAME_END)
        {
            DialogueManager.DialogueUnit endDialouge = new DialogueManager.DialogueUnit("왕", "이제 집에 가자");
            DialogueManager.Instance.AddDialogue(endDialouge);

        }
        else
        {
            
            Piece subject = chessEvent.Subject;
            talk = GetDialogue(subject.character, chessEvent.Type);
            Debug.Log(string.Format("{0} : {1}", subject, talk));
            DialogueManager.DialogueUnit dialogue = new DialogueManager.DialogueUnit(subject, talk);
            // 캐릭터 대사 가져오기 

            DialogueManager.Instance.AddDialogue(dialogue);

            if (chessEvent.Target != null)
            {
                subject = chessEvent.Target;
                talk = GetResponseDialogue(subject.character, chessEvent.Type);
                Debug.Log(string.Format("{0} : {1}", subject, talk));
                DialogueManager.DialogueUnit dialogue2 = new DialogueManager.DialogueUnit(subject, talk);
                DialogueManager.Instance.AddDialogue(dialogue2);
                // 캐릭터 반응 가져오기
            }
        }

        // 나머지 등록된 이벤트 순회.
        // 참가자로 등록 시키기.
        // 혹은 후속 대사로 등록 시키기.

        eventList.Clear();
        eventsDebug.Clear();
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
                return "왕을 공격하겠다!";
                //return "Check";
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
                return "감히!";
            default:
                return "Default";
        }
    }



}
