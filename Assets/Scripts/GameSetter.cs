using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetter : MonoBehaviour
{

    // �����ʹ� �̸� �־�дٰ� �����Ѵ�.
    public List<GameData> gameSettings;

    public AI aiManager;


/*    private void Awake()
    {
        gameSettings = new List<GameData>();
    }*/


    public void SetOpponents(int index = 0)
    {
        if (gameSettings.Count <= index) return;

        Debug.Log("���� ����");

        Board mainBoard = GameObject.Find("MainBoard").GetComponent<Board>();

        Debug.Log("ã�Ƴ� ����: " + mainBoard);

        GameData setting = gameSettings[index];

        for (int i = 0; i < setting.opponents.Count; i++)
        {
            GameData.OpponentPiece opponent = setting.opponents[i];
            Piece instance = Instantiate(opponent.piecePrefab);

            Debug.Log("��ġ���� �⹰: " + opponent.piecePrefab);

            instance.team = setting.opponentTeam;
            instance.character = opponent.character;
            instance.SetInPlace(mainBoard.GetPlace(opponent.location));

            aiManager.AddAIPiece(instance);
        }

        for (int i = 0; i < setting.players.Count; i++)
        {
            GameData.PlayerPiece player = setting.players[i];
            Piece instance = Instantiate(player.piecePrefab);

            Debug.Log("��ġ���� �⹰: " + player.piecePrefab);

            instance.team = setting.playerTeam;
            instance.character = player.character;

            PlayerDataManager.Instance.AddPlayerPiece(instance);

            instance.SetInPlace(mainBoard.GetPlace(player.location));


        }
    }

    public void SetPlayers(int index = 0)
    {

        if (gameSettings.Count <= index) return;

        Debug.Log("���� ����");

        //Board mainBoard = GameObject.Find("MainBoard").GetComponent<Board>();

        //Debug.Log("ã�Ƴ� ����: " + mainBoard);

        GameData setting = gameSettings[index];

        for (int i = 0; i < setting.players.Count; i++)
        {
            GameData.PlayerPiece player = setting.players[i];
            Piece instance = player.piecePrefab;
            CharacterData character = player.character;

            //Debug.Log("��ġ���� �⹰: " + players.piecePrefab);

            instance.team = setting.playerTeam;
            instance.character = player.character;

            // ��򰡿� ��ġ ���Ѿ� �Ѵ�.

            PlayerDataManager.Instance.AddPlayerPiece(instance);

            //instance.SetInPlace(mainBoard.GetPlace(players.location));


        }
    }

}
