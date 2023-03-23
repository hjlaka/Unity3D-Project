using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;

public class GameSetter : MonoBehaviour
{

    // �����ʹ� �̸� �־�дٰ� �����Ѵ�.
    public List<GameData> gameSettings;

    public UnityEvent OnOpponentSet;

    private Board mainBoard;
    private ListBoard initBoard;

    private void Start()
    {
        mainBoard = GameObject.Find("MainBoard").GetComponent<Board>();
        Debug.Log(string.Format("ã�Ƴ� ����: {0}", mainBoard));

        initBoard = GameObject.Find("HandMadeBoard").GetComponent<ListBoard>();
    }

    public void SetTopTeam(int index = 0)
    {
        if (gameSettings.Count <= index) return;

        Debug.Log("���� ����");

        GameData setting = gameSettings[index];
        List<GameData.CallingPiece> callings = setting.opponents;
        Player teamPlayer = GameManager.Instance.OpponentPlayer;

        Debug.Log(string.Format("��ġ�� �⹰ ��: {0}", callings.Count));
        for (int i = 0; i < callings.Count; i++)
        {
            GameData.CallingPiece calling = callings[i];
            Piece instance = CreatePiece(calling, teamPlayer);

            instance.team = setting.opponentTeam;
            Place targetPlace = mainBoard.GetPlace(calling.location);
            instance.place = targetPlace;
            instance.IsOnGame = true;
            SetSettingDialogue(instance, calling);

            instance.SetOnGame();

        }
        OnOpponentSet?.Invoke();

    }

    public void SetBottomTeam(int index = 0)
    {

        if (gameSettings.Count <= index) return;

        Debug.Log("���� ����");



        GameData setting = gameSettings[index];
        List<GameData.CallingPiece> callings = setting.players;
        Player teamPlayer = GameManager.Instance.Player;

        for (int i = 0; i < callings.Count; i++)
        {
            GameData.CallingPiece calling = callings[i];
            Piece instance = CreatePiece(calling, teamPlayer);

            instance.team = setting.playerTeam;
            Place targetPlace = initBoard.AutoAddPiece(instance);
            if (null == targetPlace)
            {
                Debug.LogError("��ȯ�� �� �ִ� �⹰���� �� ���� �⹰ ��ȯ");
                return;
            }
            instance.place = targetPlace;
            instance.IsOnGame = true;


            PlayerDataManager.Instance.AddPlayerPiece(instance);

            instance.SetOnGame();


        }
    }

    private Piece CreatePiece(GameData.CallingPiece calling, Player teamPlayer)
    {
        //Debug.Log("��ġ���� �⹰: " + calling.piecePrefab);

        Piece instance = Instantiate(calling.piecePrefab);
        instance.name = calling.piecePrefab.name;
        instance.transform.parent = teamPlayer.transform;
        instance.character = calling.character;

        instance.BelongTo(teamPlayer);

        if (calling.isCoreUnit)
            teamPlayer.CoreUnit = instance;


        return instance;

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

        for (int i = 0; i < calledgDialogue.Count; i++)
        {
            dialogue = calledgDialogue[i];
            DialogueManager.DialogueUnit dialogueUnit = new DialogueManager.DialogueUnit(unit, dialogue);
            DialogueManager.Instance.AddDialogue(dialogueUnit);
        }

    }
}


