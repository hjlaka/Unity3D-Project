using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOnTurn : StateBehaviour<GameManager>
{
    //TODO: ����
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
        //������ �� �ִ� ����� ���� �ִ��� Ȯ�� (������ ���º�)
        ChessEventManager.Instance.GetEvent();
        DialogueManager.Instance.CheckDialogueEvent(TurnStart);
        
    }

    public override void StateExit()
    {
        // üũ �̺�Ʈ
        // ��ȭ �̺�Ʈ
    }

    private void TurnStart()
    {
        machine.TurnActionDecided = true;
        Debug.Log("�� ���� ���� ���·� ����");

        if (machine.curPlayer is AI)
        {
            ((AI)machine.curPlayer).DoTurn();
        }
    }
}
