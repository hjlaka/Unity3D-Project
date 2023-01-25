using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public enum GameState
    {
        START_GAME,
        SETTING_GAME,
        PREPARING_GAME,
        SELECTING_PIECE,
        SELECTING_PLACE,
        DOING_PLAYER_TURN,
        TURN_FINISHED,
        TURN_CHANGE,
        CHECK_EVENT,
        IN_CONVERSATION,
        OPPONENT_TURN,
        RETURN,
        GAME_END,
        OUT_OF_GAME
    }

    protected GameManager manager = GameManager.Instance;
}
