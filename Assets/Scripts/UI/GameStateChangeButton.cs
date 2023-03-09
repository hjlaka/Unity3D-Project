using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateChangeButton : MonoBehaviour
{
    [SerializeField]
    private GameManager.GameState targetState;

    public void ChangeGameState()
    {
        //GameManager.Instance.ChangeGameState(targetState);
        GameManager.Instance.SetNextState(targetState);
    }

    public void GoBackGameState()
    {
        GameManager.Instance.GoBackGameState();
    }
}
