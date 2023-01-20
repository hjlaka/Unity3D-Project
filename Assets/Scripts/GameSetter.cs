using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSetter : MonoBehaviour
{

    // 데이터는 미리 넣어둔다고 가정한다.
    public List<GameData> gameSettings;

    public AI aiManager;

    public UnityEvent OnOpponentSet;


    public void SetTopTeam(int index = 0)
    {
        if (gameSettings.Count <= index) return;

        Debug.Log("세팅 시작");

        Board mainBoard = GameObject.Find("MainBoard").GetComponent<Board>();

        Debug.Log("찾아낸 보드: " + mainBoard);

        GameData setting = gameSettings[index];

        Debug.Log("배치할 기물 수: " + setting.opponents.Count);
        for (int i = 0; i < setting.opponents.Count; i++)
        {
            GameData.CallingPiece opponent = setting.opponents[i];
            Piece instance = Instantiate(opponent.piecePrefab);

            Debug.Log("배치중인 기물: " + opponent.piecePrefab);

            instance.team = setting.opponentTeam;
            instance.character = opponent.character;

            Place targetPlace = mainBoard.GetPlace(opponent.location);
            instance.place = targetPlace;


            aiManager.AddPiece(instance);
        }
        OnOpponentSet?.Invoke();

    }

    public void SetBottomTeam(int index = 0)
    {

        if (gameSettings.Count <= index) return;

        Debug.Log("세팅 시작");

        Board initBoard = GameObject.Find("InitialBoard").GetComponent<Board>();

        GameData setting = gameSettings[index];

        for (int i = 0; i < setting.players.Count; i++)
        {
            GameData.CallingPiece player = setting.players[i];
            Debug.Log("배치중인 기물: " + player.piecePrefab);

            Piece instance = Instantiate(player.piecePrefab);
            instance.team = setting.playerTeam;
            instance.character = player.character;

            int randX = Random.Range(0, 3);
            int randY = Random.Range(0, 3);

            Debug.Log("랜덤 위치: " + randX + ", " + randY);
            Place targetPlace = initBoard.GetPlace(new Vector2Int(randX, randY));
            instance.place = targetPlace;
            //PlaceManager.Instance.MovePiece(instance, targetPlace);

            PlayerDataManager.Instance.AddPlayerPiece(instance);

        }
    }

}
