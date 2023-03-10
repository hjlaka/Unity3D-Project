using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTurnFinished : StateBehaviour<GameManager>
{
    //TODO: ����
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
            Debug.Log("���� ���� �ν�");
            ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.GAME_END, null, null));
            ChangeGameState(GameState.GAME_END);
        
        }*/

        // ���� ���� üũ�� ���⿡�� �ؾ� �ϳ�?
        
        if(false/* ���� ���� ���� ���� */)
        {
            // ���� ����� �Ѿ���� �ϱ�
        }
        else
        {
            //���⿡�� �̺�Ʈ üũ
            // �̺�Ʈ ����
            // ü�� �̺�Ʈ
            ChessEventManager.Instance.GetEvent();

            // ��ȭ �̺�Ʈ
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

    // �ӽ� �߰�
    private void NextStep()
    {
        machine.SetNextState(GameManager.GameState.ON_TURN);
    }
}
