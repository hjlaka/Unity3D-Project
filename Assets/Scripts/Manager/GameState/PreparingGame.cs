using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparingGame : GameStateMachine
{
    public override void StateEnter()
    {
        manager.gameSetter.SetBottomTeam(0);
        PlayerDataManager.Instance.EnablePlayerListUI();
        manager.playerSetter.GetBoard();
        manager.playerSetter.MakeBoardSetable();
    }

    public override void StateExit()
    {
        manager.playerSetter.MakeBoardSetableNot();
        PlayerDataManager.Instance.DisablePlayerListUI();
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }
}
