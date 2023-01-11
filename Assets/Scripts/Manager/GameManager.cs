using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public enum GameState { SELECTING_PIECE, SELECTING_PLACE, TURN_CHANGE, IN_CONVERSATION, TURN_FINISHED, AI_TURN}
    public GameState state;

    public bool isPlayerTurn;


    [SerializeField]
    private GameSetter gameSetter;


    [SerializeField]
    private int trunRemain;
    public int TurnRemain
    {
        get { return trunRemain; }
        set 
        { 
            trunRemain = value;
            turnRemainUI.text = trunRemain.ToString();
        }
    }    
    [SerializeField]
    private TextMeshProUGUI turnRemainUI;

    private void Start()
    {
        TurnRemain = 10;
        gameSetter.SetPlayers(0);

    }

    public void TurnCalculate()
    {

    }


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
