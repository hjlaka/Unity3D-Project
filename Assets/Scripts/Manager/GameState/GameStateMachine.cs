using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateMachine : MonoBehaviour, IState
{

    protected GameManager manager = GameManager.Instance;

    public abstract void StateEnter();


    public abstract void StateExit();


    public abstract void StateUpdate();

}
