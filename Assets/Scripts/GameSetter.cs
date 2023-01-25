using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSetter : MonoBehaviour
{

    // �����ʹ� �̸� �־�дٰ� �����Ѵ�.
    public List<GameData> gameSettings;

    public UnityEvent OnOpponentSet;


    public void SetTopTeam(int index = 0)
    {
        if (gameSettings.Count <= index) return;

        Debug.Log("���� ����");

        Board mainBoard = GameObject.Find("MainBoard").GetComponent<Board>();

        Debug.Log("ã�Ƴ� ����: " + mainBoard);

        GameData setting = gameSettings[index];
        List<GameData.CallingPiece> callings = setting.opponents;
        Player teamPlayer = GameManager.Instance.OpponentPlayer;

        Debug.Log("��ġ�� �⹰ ��: " + callings.Count);
        for (int i = 0; i < callings.Count; i++)
        {
            GameData.CallingPiece calling = callings[i];
            Piece instance = Instantiate(calling.piecePrefab);

            Debug.Log("��ġ���� �⹰: " + calling.piecePrefab);

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

        Debug.Log("���� ����");

        //Board initBoard = GameObject.Find("InitialBoard").GetComponent<Board>();
        Board initBoard = GameObject.Find("HandMadeBoard").GetComponent<Board>();

        GameData setting = gameSettings[index];
        List<GameData.CallingPiece> callings = setting.players;
        Player teamPlayer = GameManager.Instance.Player;

        for (int i = 0; i < callings.Count; i++)
        {
            GameData.CallingPiece calling = callings[i];
            Debug.Log("��ġ���� �⹰: " + calling.piecePrefab);

            Piece instance = Instantiate(calling.piecePrefab);
            instance.team = setting.playerTeam;
            instance.character = calling.character;

            //int randX = Random.Range(0, 3);
            //int randY = Random.Range(0, 3);

            //Debug.Log("���� ��ġ: " + randX + ", " + randY);
            Place targetPlace = initBoard.transform.GetChild(i).GetComponent<Place>();
            instance.place = targetPlace;
            instance.IsOnGame = true;
            //PlaceManager.Instance.MovePiece(instance, targetPlace);

            PlayerDataManager.Instance.AddPlayerPiece(instance);

            Debug.Log("�÷��̾�: " + teamPlayer);
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
