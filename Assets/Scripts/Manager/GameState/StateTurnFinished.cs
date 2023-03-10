using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTurnFinished : StateBehaviour<GameManager>
{
    //TODO: 수정
    public override StateBehaviour<GameManager> Handle()
    {
        switch (machine.NextStateType)
        {
            case GameManager.GameState.ON_TURN:
                break;
            default:
                return null;
        }

        machine.ChangeGameStateMachine();
        return null;
    }
    public override void StateEnter()
    {
        /*if (machine.IsEnded())
        {
            Debug.Log("게임 종료 인식");
            ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.GAME_END, null, null));
            ChangeGameState(GameState.GAME_END);
        
        }*/

        // 게임 종료 체크도 여기에서 해야 하나?
        
        if(false/* 게임 종료 조건 충족 */)
        {
            // 게임 종료로 넘어가도록 하기
        }
        else
        {
            //여기에서 이벤트 체크
            // 이벤트 실행
            // 체스 이벤트
            ChessEventManager.Instance.GetEvent();

            // 대화 이벤트
            DialogueManager.Instance.CheckDialogueEvent(NextStep);
        }
    }

    public override void StateExit()
    {
        //OnFinishTurn?.
        machine.ChangeTurn();
    }

    public override void StateUpdate()
    {
        
    }

    // 임시 추가
    private void NextStep()
    {
        machine.SetNextState(GameManager.GameState.ON_TURN);
    }
}
