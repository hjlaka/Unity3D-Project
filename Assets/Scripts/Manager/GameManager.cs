using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public enum GameState { SELECTING_PIECE, SELECTING_PLACE, TURN_CHANGE }
    public GameState state;


    public void GameStateUpdate()
    {
        switch(state)
        {
            case GameState.SELECTING_PIECE:
                break;

            case GameState.SELECTING_PLACE:
                break;

            case GameState.TURN_CHANGE:
                break;
                
            default: 
                state = GameState.TURN_CHANGE; 
                break;
        }
    }

}
