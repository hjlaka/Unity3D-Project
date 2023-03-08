using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOnTurn : StateBehaviour<GameManager>
{
    public override void StateEnter()
    {
        ChessEventManager.Instance.GetEvent();
        DialogueManager.Instance.CheckDialogueEvent();
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate()
    {
        
    }
}
