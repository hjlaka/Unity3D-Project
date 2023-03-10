using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateGameStart : StateBehaviour<GameManager>
{
    private string debug1 = "StateEnter 작업 완료";

    public override StateBehaviour<GameManager> Handle()
    {
        switch(machine.NextStateType)
        {
            case GameManager.GameState.SETTING_GAME:
                break;
            default:
                return null;
        }

        machine.ChangeGameStateMachine();
        return null;
    }
    public override void StateEnter()
    {
        machine.OnTest?.Invoke();
        ApplyPlayerType();
        ApplyOpponentType();
        ApplyBothPlayerDirection();
        machine.curPlayer = machine.bottomPlayer;

        machine.SetNextState(GameManager.GameState.SETTING_GAME);


        Debug.Log(debug1);
    }
           
    public override void StateExit()
    {
    }

    public override void StateUpdate()
    {

    }

    private void ApplyPlayerType()
    {
        switch (machine.playerType)
        {
            case GameManager.PlayerType.AI:
                GameObject playerObj = machine.Player.gameObject;
                Destroy(machine.Player);
                playerObj.AddComponent<AI>();

                machine.player = playerObj.GetComponent<AI>();
                break;

            case GameManager.PlayerType.PLAYER:
                break;

            case GameManager.PlayerType.PLAYER2:
                GameObject player2Player = new GameObject();
                player2Player.AddComponent<Player>();

                machine.player = player2Player.GetComponent<Player>();
                break;
        }

    }
    private void ApplyOpponentType()
    {
        switch (machine.opponentType)
        {
            case GameManager.PlayerType.AI:
                machine.opponentPlayer = machine.aiManager;
                break;
            case GameManager.PlayerType.PLAYER:
                machine.opponentPlayer = machine.Player;
                break;
            case GameManager.PlayerType.PLAYER2:
                GameObject player2Opponent = new GameObject();
                player2Opponent.AddComponent<Player>();
                machine.opponentPlayer = player2Opponent.GetComponent<Player>();
                break;
        }

    }

    private void ApplyBothPlayerDirection()
    {
        // 상대편이 결정된 다음에 동작할 것.
        switch (machine.playerTeamDirection)
        {
            case TeamData.Direction.UpToDown:

                machine.topPlayer = machine.Player;
                machine.bottomPlayer = machine.OpponentPlayer;
                break;

            case TeamData.Direction.DownToUp:

                machine.topPlayer = machine.OpponentPlayer;
                machine.bottomPlayer = machine.Player;
                break;
        }

        machine.topPlayer.homeLocation = machine.topHome;
        machine.bottomPlayer.homeLocation = machine.bottomHome;
    }
}


