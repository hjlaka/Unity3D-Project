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

        machine.ChangeGameStateMachine();
        return null;
    }
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
        while(DialogueManager.Instance.inConversation)
        {
            yield return null;
        }
        machine.SetNextState(GameManager.GameState.PREPARING_GAME);
    }
}


