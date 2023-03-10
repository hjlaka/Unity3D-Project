using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTurnAction : StateBehaviour<GameManager>
{
    //TODO: ¼öÁ¤
    public override StateBehaviour<GameManager> Handle()
    {
        switch (machine.NextStateType)
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
        //
    }

    public override void StateExit()
    {
        //
    }
}
