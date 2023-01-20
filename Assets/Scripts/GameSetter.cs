using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSetter : MonoBehaviour
{

    // �����ʹ� �̸� �־�дٰ� �����Ѵ�.
    public List<GameData> gameSettings;

    public AI aiManager;

    public UnityEvent OnOpponentSet;


    public void SetOpponents(int index = 0)
    {
        if (gameSettings.Count <= index) return;

        Debug.Log("���� ����");

        Board mainBoard = GameObject.Find("MainBoard").GetComponent<Board>();

        Debug.Log("ã�Ƴ� ����: " + mainBoard);

        GameData setting = gameSettings[index];

        Debug.Log("��ġ�� �⹰ ��: " + setting.opponents.Count);
        for (int i = 0; i < setting.opponents.Count; i++)
        {
            GameData.OpponentPiece opponent = setting.opponents[i];
            Piece instance = Instantiate(opponent.piecePrefab);

            Debug.Log("��ġ���� �⹰: " + opponent.piecePrefab);

            instance.team = setting.opponentTeam;
            instance.character = opponent.character;
            //instance.SetInPlace(mainBoard.GetPlace(opponent.location));
            Place setPlace = mainBoard.GetPlace(opponent.location);
            instance.place = setPlace;

            //PlaceManager.Instance.MovePiece(instance, place);

            //������ ����
            //PlaceManager.Instance.CalculateInfluence(instance);

            aiManager.AddAIPiece(instance);
        }
        OnOpponentSet?.Invoke();

    }

    public void SetPlayers(int index = 0)
    {

        if (gameSettings.Count <= index) return;

        Debug.Log("���� ����");

        Board initBoard = GameObject.Find("InitialBoard").GetComponent<Board>();

        //Debug.Log("ã�Ƴ� ����: " + mainBoard);

        GameData setting = gameSettings[index];

        for (int i = 0; i < setting.players.Count; i++)
        {
            GameData.PlayerPiece player = setting.players[i];
            Debug.Log("��ġ���� �⹰: " + player.piecePrefab);

            Piece instance = Instantiate(player.piecePrefab);
            instance.team = setting.playerTeam;
            instance.character = player.character;

            int randX = Random.Range(0, 3);
            int randY = Random.Range(0, 3);

            Debug.Log("���� ��ġ: " + randX + ", " + randY);
            //instance.SetInPlace(initBoard.GetPlace(new Vector2Int(randX, randY)));
            instance.place = initBoard.GetPlace(new Vector2Int(randX, randY));

            PlayerDataManager.Instance.AddPlayerPiece(instance);

        }
    }

}
