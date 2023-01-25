using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSetter : MonoBehaviour
{

    // 데이터는 미리 넣어둔다고 가정한다.
    public List<GameData> gameSettings;

    public UnityEvent OnOpponentSet;


    public void SetTopTeam(int index = 0)
    {
        if (gameSettings.Count <= index) return;

        Debug.Log("세팅 시작");

        Board mainBoard = GameObject.Find("MainBoard").GetComponent<Board>();

        Debug.Log("찾아낸 보드: " + mainBoard);

        GameData setting = gameSettings[index];
        List<GameData.CallingPiece> callings = setting.opponents;
        Player teamPlayer = GameManager.Instance.OpponentPlayer;

        Debug.Log("배치할 기물 수: " + callings.Count);
        for (int i = 0; i < callings.Count; i++)
        {
            GameData.CallingPiece calling = callings[i];
            Piece instance = Instantiate(calling.piecePrefab);

            Debug.Log("배치중인 기물: " + calling.piecePrefab);

            instance.team = setting.opponentTeam;
            instance.character = calling.character;

            Place targetPlace = mainBoard.GetPlace(calling.location);
            instance.place = targetPlace;
            instance.IsOnGame = true;
            SetSettingDialogue(instance, calling);

            instance.BelongTo(teamPlayer);

            if (calling.isCoreUnit)
                teamPlayer.CoreUnit = instance;

        }
        OnOpponentSet?.Invoke();

    }

    public void SetBottomTeam(int index = 0)
    {

        if (gameSettings.Count <= index) return;

        Debug.Log("세팅 시작");

        //Board initBoard = GameObject.Find("InitialBoard").GetComponent<Board>();
        Board initBoard = GameObject.Find("HandMadeBoard").GetComponent<Board>();

        GameData setting = gameSettings[index];
        List<GameData.CallingPiece> callings = setting.players;
        Player teamPlayer = GameManager.Instance.Player;

        for (int i = 0; i < callings.Count; i++)
        {
            GameData.CallingPiece calling = callings[i];
            Debug.Log("배치중인 기물: " + calling.piecePrefab);

            Piece instance = Instantiate(calling.piecePrefab);
            instance.team = setting.playerTeam;
            instance.character = calling.character;

            //int randX = Random.Range(0, 3);
            //int randY = Random.Range(0, 3);

            //Debug.Log("랜덤 위치: " + randX + ", " + randY);
            Place targetPlace = initBoard.transform.GetChild(i).GetComponent<Place>();
            instance.place = targetPlace;
            instance.IsOnGame = true;
            //PlaceManager.Instance.MovePiece(instance, targetPlace);

            PlayerDataManager.Instance.AddPlayerPiece(instance);

            Debug.Log("플레이어: " + teamPlayer);
            instance.BelongTo(teamPlayer);

            if (calling.isCoreUnit)
                teamPlayer.CoreUnit = instance;


        }
    }

    public void SetSettingEvent(int index = 0)
    {
        if (gameSettings.Count <= index) return;

        List<EventDialogue.EventDialogueUnit> eventDialogues = gameSettings[index].eventDialogue.eventDialogues;

        for (int i = 0; i < eventDialogues.Count; i++)
        {
            DialogueManager.DialogueUnit dialogueUnit = new DialogueManager.DialogueUnit(eventDialogues[i].unitName, eventDialogues[i].dialogue);
            DialogueManager.Instance.AddDialogue(dialogueUnit);
        }
    }

    private void SetSettingDialogue(Unit unit, GameData.CallingPiece calling)
    {
        List<string> calledgDialogue = calling.calledDialogue;
        string dialogue;

        for(int i = 0; i < calledgDialogue.Count; i++)
        {
            dialogue = calledgDialogue[i];
            DialogueManager.DialogueUnit dialogueUnit = new DialogueManager.DialogueUnit(unit, dialogue);
            DialogueManager.Instance.AddDialogue(dialogueUnit);
        }
        
    }

}
