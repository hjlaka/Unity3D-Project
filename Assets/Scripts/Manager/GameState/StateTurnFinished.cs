using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateTurnFinished : StateBehaviour<GameManager>
{

    private GameManager.GameState nextState;
    [SerializeField]
    private GameEndUI gameEndUI;
    private string resultText;

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

        machine.UpdateGameStateMachine();
        return null;
    }
    public override void StateEnter()
    {
        switch(machine.JudgeGame())
        {
            case GameManager.Result.WIN:
                ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.GAME_END, null, null));
                nextState = GameManager.GameState.ON_PEACE;
                string winName = machine.curPlayer == machine.player ? "플레이어" : "상대편";
                resultText = string.Format("{0} 승리!", winName);
                break;
            case GameManager.Result.DRAW:
                ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.GAME_END, null, null));
                nextState = GameManager.GameState.ON_PEACE;
                resultText = string.Format("무승부!");
                break;
            case GameManager.Result.NONE:
                nextState = GameManager.GameState.ON_TURN;
                break;
            default:
                break;
        }

        //Debug.Log(string.Format"이벤트 실행 : 이벤트 개수 {0}", );
        ChessEventManager.Instance.GetEvent();

        // 대화 이벤트
        DialogueManager.Instance.CheckDialogueEvent(NextStep);
    }

    public override void StateExit()
    {
        //OnFinishTurn?.
        machine.ChangeTurn();
    }

    // 임시 추가
    private void NextStep()
    {
        if(nextState == GameManager.GameState.ON_PEACE)
        {
            gameEndUI.UpdateUI(resultText);
        }
        machine.SetNextState(nextState);
    }
}
