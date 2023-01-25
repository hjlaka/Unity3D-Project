using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class GameStart : GameStateMachine, IState
    {
          public void StateEnter()
          {
              manager.OnTest?.Invoke();
              ApplyPlayerType();
              ApplyOpponentType();
              ApplyBothPlayerDirection();
              manager.CurPlayer = manager.bottomPlayer;
          }

          public void StateExit()
          {
            //manager.ChangeGameState(GameState.SETTING_GAME);
          }

          public void StateUpdate()
          {

          }


        private void ApplyPlayerType()
        {
            switch (manager.playerType)
            {
                case GameManager.PlayerType.AI:
                    GameObject playerObj = manager.Player.gameObject;
                    Destroy(manager.Player);
                    playerObj.AddComponent<AI>();

                    manager.Player = playerObj.GetComponent<AI>();
                    break;

                case GameManager.PlayerType.PLAYER:
                    break;

                case GameManager.PlayerType.PLAYER2:
                    GameObject player2Player = new GameObject();
                    player2Player.AddComponent<Player>();

                    manager.Player = player2Player.GetComponent<Player>();
                    break;
            }

        }
        private void ApplyOpponentType()
        {
            switch (manager.opponentType)
            {
                case GameManager.PlayerType.AI:
                    manager.OpponentPlayer = manager.aiManager;
                    break;
                case GameManager.PlayerType.PLAYER:
                    manager.OpponentPlayer = manager.Player;
                    break;
                case GameManager.PlayerType.PLAYER2:
                    GameObject player2Opponent = new GameObject();
                    player2Opponent.AddComponent<Player>();
                    manager.OpponentPlayer = player2Opponent.GetComponent<Player>();
                    break;
            }

        }

        private void ApplyBothPlayerDirection()
        {
            // 상대편이 결정된 다음에 동작할 것.
            switch (manager.playerTeamDirection)
            {
                case TeamData.Direction.UpToDown:

                    manager.topPlayer = manager.Player;
                    manager.bottomPlayer = manager.OpponentPlayer;
                    break;

                case TeamData.Direction.DownToUp:

                    manager.topPlayer = manager.OpponentPlayer;
                    manager.bottomPlayer = manager.Player;
                    break;
            }
        }

    }
}

