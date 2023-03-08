using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePreparingGame : StateBehaviour<GameManager>
{
    public override void StateEnter()
    {
        machine.gameSetter.SetBottomTeam(0);
        PlayerDataManager.Instance.EnablePlayerListUI();
        machine.playerSetter.GetBoard();
        machine.playerSetter.MakeBoardSetable();
    }

    public override void StateExit()
    {
        machine.playerSetter.MakeBoardSetableNot();
        PlayerDataManager.Instance.DisablePlayerListUI();
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }
}
