using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetter : MonoBehaviour
{

    // �����ʹ� �̸� �־�дٰ� �����Ѵ�.
    public List<GameData> gameSettings;

    public AI aiManager;
    public PlayerData playerData;


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
            instance.charactor = opponent.character;
            instance.SetInPlace(mainBoard.GetPlace(opponent.location));

            aiManager.AddAIPiece(instance);
        }

        for (int i = 0; i < setting.players.Count; i++)
        {
            GameData.PlayerPiece players = setting.players[i];
            Piece instance = Instantiate(players.piecePrefab);

            Debug.Log("��ġ���� �⹰: " + players.piecePrefab);

            instance.team = setting.playerTeam;
            instance.charactor = players.character;
            instance.SetInPlace(mainBoard.GetPlace(players.location));

        }
    }

    public void SetPlayers(int index = 0)
    {

    }

}
