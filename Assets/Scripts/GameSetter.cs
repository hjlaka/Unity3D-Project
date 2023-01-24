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

        Debug.Log("��ġ�� �⹰ ��: " + setting.opponents.Count);
        for (int i = 0; i < setting.opponents.Count; i++)
        {
            GameData.CallingPiece calling = setting.opponents[i];
            Piece instance = Instantiate(calling.piecePrefab);

            Debug.Log("��ġ���� �⹰: " + calling.piecePrefab);

            instance.team = setting.opponentTeam;
            instance.character = calling.character;

            Place targetPlace = mainBoard.GetPlace(calling.location);
            instance.place = targetPlace;
            SetSettingDialogue(instance, calling);

            instance.BelongTo(GameManager.Instance.OpponentPlayer);

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

        for (int i = 0; i < setting.players.Count; i++)
        {
            GameData.CallingPiece player = setting.players[i];
            Debug.Log("��ġ���� �⹰: " + player.piecePrefab);

            Piece instance = Instantiate(player.piecePrefab);
            instance.team = setting.playerTeam;
            instance.character = player.character;

            //int randX = Random.Range(0, 3);
            //int randY = Random.Range(0, 3);

            //Debug.Log("���� ��ġ: " + randX + ", " + randY);
            Place targetPlace = initBoard.transform.GetChild(i).GetComponent<Place>();
            instance.place = targetPlace;
            //PlaceManager.Instance.MovePiece(instance, targetPlace);

            PlayerDataManager.Instance.AddPlayerPiece(instance);

            Debug.Log("�÷��̾�: " + GameManager.Instance.Player);
            instance.BelongTo(GameManager.Instance.Player);


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
