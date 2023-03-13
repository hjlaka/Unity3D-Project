using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePeace : StateBehaviour<GameManager>
{
    public override StateBehaviour<GameManager> Handle()
    {
        switch (machine.NextStateType)
        {
            case GameManager.GameState.START_GAME:
                break;
            default:
                return null;
        }

        machine.UpdateGameStateMachine();
        return null;
    }

    public override void StateEnter()
    {

    }

    public override void StateExit()
    {

    }
}
