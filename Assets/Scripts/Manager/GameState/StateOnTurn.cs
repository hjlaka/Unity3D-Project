using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOnTurn : StateBehaviour<GameManager>
{
    //TODO: 수정
    public override StateBehaviour<GameManager> Handle()
    {
        switch (machine.NextStateType)
        {
            case GameManager.GameState.TURN_FINISHED:
                break;
            default:
                return null;
        }

        machine.UpdateGameStateMachine();
        return null;
    }
    public override void StateEnter()
    {
        //TODO:
        //움직일 수 있는 경우의 수가 있는지 확인 (없으면 무승부)

        machine.playerValidToSelectPlace = true;
        ChessEventManager.Instance.GetEvent();
        DialogueManager.Instance.CheckDialogueEvent();
    }

    public override void StateExit()
    {
        // 체크 이벤트
        // 대화 이벤트
    }

    public override void StateUpdate()
    {
        
    }
}
