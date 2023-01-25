using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class PrepareingGame : GameStateMachine, IState
    {
        public void StateEnter()
        {
            PlayerDataManager.Instance.EnablePlayerListUI();
            DialogueManager.Instance.CheckDialogueEvent();
            manager.gameSetter.SetBottomTeam(0);
        }

        public void StateExit()
        {
            //ChangeGameState(GameState.SELECTING_PIECE);
            PlayerDataManager.Instance.DisablePlayerListUI();
        }

        public void StateUpdate()
        {
            
        }
    }
}
