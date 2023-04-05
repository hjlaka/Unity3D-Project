using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateSettingGame : StateBehaviour<GameManager>
{
    //TODO: ¼öÁ¤
    public override StateBehaviour<GameManager> Handle()
    {
        switch (machine.NextStateType)
        {
            case GameManager.GameState.PREPARING_GAME:
                break;
            default:
                return null;
        }

        machine.UpdateGameStateMachine();
        return null;
    }
    public override void StateEnter()
    {
        DialogueManager.Instance.CheckDialogueEvent(NextStep);
        machine.gameSetter.SetTopTeam(0);
    }

    public override void StateExit()
    {
        //ChangeGameState(GameState.PREPARING_GAME);
    }

    private void NextStep()
    {
        machine.SetNextState(GameManager.GameState.PREPARING_GAME);
    }
}


