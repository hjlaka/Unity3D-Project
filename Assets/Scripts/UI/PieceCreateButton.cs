using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangeLayerFunc))]
public class PieceCreateButton : MonoBehaviour
{
    [SerializeField]
    private Piece piecePrefab;
    [SerializeField]
    private TeamData team;
    [SerializeField]
    private CharacterData testCharacter;

    [SerializeField]
    private AI aiManager;

    [SerializeField]
    private ListBoard listBoard;

    public void CreatePiece()
    {

        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE &&
            GameManager.Instance.state != GameManager.GameState.PREPARING_GAME_ON)
        {
            return;
        }

        Debug.Log("기물을 생성했습니다." + piecePrefab.name);
        Piece instance = Instantiate(piecePrefab);

        instance.team = team;
        instance.character = testCharacter;


        Debug.Log("전달한 것: " + instance);


        if (team.direction == TeamData.Direction.UpToDown)
        {
            GameManager.Instance.OpponentPlayer.AddPiece(instance);
            // 테스트를 위해 생성된 기물의 조작은 플레이어가 함

            instance.Belong = GameManager.Instance.Player;
            instance.transform.parent = GameManager.Instance.OpponentPlayer.transform;
        }
        else
        {
            PlayerDataManager.Instance.AddPlayerPiece(instance);
            instance.BelongTo(GameManager.Instance.Player);
            instance.transform.parent = GameManager.Instance.Player.transform;
        }
    }

    

}

    
