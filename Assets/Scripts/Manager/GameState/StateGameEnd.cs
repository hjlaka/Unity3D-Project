using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameEnd : StateBehaviour<GameManager>
{
    public override StateBehaviour<GameManager> Handle()
    {
        switch (machine.NextStateType)
        {
            case GameManager.GameState.ON_PEACE:
                break;
            default:
                return null;
        }

        machine.ChangeGameStateMachine();
        return null;
    }

    public override void StateEnter()
    {
        
    }

    public override void StateExit()
    {
        
    }
}
