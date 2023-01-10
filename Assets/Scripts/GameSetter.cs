using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetter : MonoBehaviour
{

    // 데이터는 미리 넣어둔다고 가정한다.
    public List<GameData> gameSettings;


/*    private void Awake()
    {
        gameSettings = new List<GameData>();
    }*/


    public void SetOpponents()
    {
        if (gameSettings.Count <= 0) return;

        Debug.Log("세팅 시작");

        Board mainBoard = GameObject.Find("MainBoard").GetComponent<Board>();

        Debug.Log("찾아낸 보드: " + mainBoard);

        GameData setting = gameSettings[0];

        for (int i = 0; i < setting.opponents.Count; i++)
        {
            GameData.OpponentPiece opponent = setting.opponents[i];
            Piece instance = Instantiate(opponent.piecePrefab);

            Debug.Log("배치중인 기물: " + opponent.piecePrefab);

            instance.team = setting.team;
            instance.charactor = opponent.character;
            instance.SetInPlace(mainBoard.GetPlace(opponent.location));

            //aiManager.AddAIPiece(instance);
        }
    }



    // 일부 기물 소환


    // 정해진 위치에 두기


    // 이 기물은 어디, 저 기물은 어디.


    // 파일로 저장?


    // 기물 종류, (캐릭터), 위치


    // 랜덤으로 생성?


    // 아군 기물은?

}
