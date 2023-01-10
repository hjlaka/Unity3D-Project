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

            instance.team = setting.team;
            instance.charactor = opponent.character;
            instance.SetInPlace(mainBoard.GetPlace(opponent.location));

            aiManager.AddAIPiece(instance);
        }
    }



    // �Ϻ� �⹰ ��ȯ


    // ������ ��ġ�� �α�


    // �� �⹰�� ���, �� �⹰�� ���.


    // ���Ϸ� ����?


    // �⹰ ����, (ĳ����), ��ġ


    // �������� ����?


    // �Ʊ� �⹰��?

}
