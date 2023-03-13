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

        machine.playerValidToSelectPlace = true;
        ChessEventManager.Instance.GetEvent();
        DialogueManager.Instance.CheckDialogueEvent();
    }

    public override void StateExit()
    {
        // üũ �̺�Ʈ
        // ��ȭ �̺�Ʈ
    }

    public override void StateUpdate()
    {
        
    }
}
