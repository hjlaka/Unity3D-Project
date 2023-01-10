using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public enum GameState { SELECTING_PIECE, SELECTING_PLACE, TURN_CHANGE, IN_CONVERSATION, TURN_FINISHED, AI_TURN}
    public GameState state;

    public bool isPlayerTurn;


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

            case GameState.IN_CONVERSATION:
                break;

            case GameState.TURN_FINISHED:
                break;

            case GameState.AI_TURN:
                break;

            default: 
                state = GameState.TURN_CHANGE; 
                break;
        }
    }

}
