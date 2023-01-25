using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class SettingGame : GameStateMachine, IState
    {
        public void StateEnter()
        {
            DialogueManager.Instance.CheckDialogueEvent();
            manager.gameSetter.SetTopTeam(0);
        }

        public void StateExit()
        {
            //ChangeGameState(GameState.PREPARING_GAME);
        }

        public void StateUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}

