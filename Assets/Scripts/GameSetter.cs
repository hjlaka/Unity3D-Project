using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetter : MonoBehaviour
{

    // �����ʹ� �̸� �־�дٰ� �����Ѵ�.
    public List<GameData> gameSettings;


/*    private void Awake()
    {
        gameSettings = new List<GameData>();
    }*/


    public void SetOpponents()
    {
        if (gameSettings.Count <= 0) return;

        Debug.Log("���� ����");

        Board mainBoard = GameObject.Find("MainBoard").GetComponent<Board>();

        Debug.Log("ã�Ƴ� ����: " + mainBoard);

        GameData setting = gameSettings[0];

        for (int i = 0; i < setting.opponents.Count; i++)
        {
            GameData.OpponentPiece opponent = setting.opponents[i];
            Piece instance = Instantiate(opponent.piecePrefab);

            Debug.Log("��ġ���� �⹰: " + opponent.piecePrefab);

            instance.team = setting.team;
            instance.charactor = opponent.character;
            instance.SetInPlace(mainBoard.GetPlace(opponent.location));

            //aiManager.AddAIPiece(instance);
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
