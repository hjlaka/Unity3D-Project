using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class PrepareingGame : GameStateMachine
    {
        public override void StateEnter()
        {
            PlayerDataManager.Instance.EnablePlayerListUI();
            DialogueManager.Instance.CheckDialogueEvent();
            manager.gameSetter.SetBottomTeam(0);
        }

        public override void StateExit()
        {
            //ChangeGameState(GameState.SELECTING_PIECE);
            PlayerDataManager.Instance.DisablePlayerListUI();
        }

        public override void StateUpdate()
        {
            
        }
    }
}
