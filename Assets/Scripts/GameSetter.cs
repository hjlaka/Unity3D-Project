using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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

        Debug.Log("세팅 시작");

        ListBoard initBoard = GameObject.Find("HandMadeBoard").GetComponent<ListBoard>();

        GameData setting = gameSettings[index];
        List<GameData.CallingPiece> callings = setting.players;
        Player teamPlayer = GameManager.Instance.Player;

        for (int i = 0; i < callings.Count; i++)
        {
            GameData.CallingPiece calling = callings[i];
            Piece instance = CreatePiece(calling, teamPlayer);

            instance.team = setting.playerTeam;
            Place targetPlace = initBoard.AutoAddPiece(instance);
            instance.place = targetPlace;
            instance.IsOnGame = true;


            PlayerDataManager.Instance.AddPlayerPiece(instance);

            instance.SetOnGame();


        }
    }

    private Piece CreatePiece(GameData.CallingPiece calling, Player teamPlayer)
    {
        Debug.Log("배치중인 기물: " + calling.piecePrefab);

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


