using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class SettingGame : GameStateMachine
    {
        public override void StateEnter()
        {
            DialogueManager.Instance.CheckDialogueEvent();
            manager.gameSetter.SetTopTeam(0);
        }

        public override void StateExit()
        {
            //ChangeGameState(GameState.PREPARING_GAME);
        }

        public override void StateUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}

