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
        // 주체가 되는 기물
        eventList.Add(chessEvent);
        
    }

    public void GetEvent()
    {
        if (eventList.Count <= 0) return;

        // 모든 이벤트 다 넣기
        for(int i = 0; i < eventList.Count; i++)
        {
            ChessEvent chessEvent = eventList[i];
            DialogueManager.DialogueUnit dialogue = new DialogueManager.DialogueUnit(chessEvent.Subject.character.name, "이벤트 1");
            // 캐릭터 대사 가져오기

            DialogueManager.Instance.AddDialogue(dialogue);

            if(chessEvent.Target != null)
            {
                DialogueManager.DialogueUnit dialogue2 = new DialogueManager.DialogueUnit(chessEvent.Target.character.name, "이벤트 2");
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


}
