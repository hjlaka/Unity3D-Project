using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class StateSettingGame : StateBehaviour<GameManager>
    {
        public override void StateEnter()
        {
            DialogueManager.Instance.CheckDialogueEvent();
            machine.gameSetter.SetTopTeam(0);

            StartCoroutine(Waiting());
        }

        public override void StateExit()
        {
            //ChangeGameState(GameState.PREPARING_GAME);
        }

        public override void StateUpdate()
        {
            throw new System.NotImplementedException();
        }

        private IEnumerator Waiting()
        {
            while(GameManager.Instance.state == GameManager.GameState.IN_CONVERSATION)
            {
                yield return null;
            }
            machine.SetNextState(GameManager.GameState.PREPARING_GAME);
        }
    }
}

