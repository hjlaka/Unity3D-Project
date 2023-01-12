using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public enum GameState 
    { 
        START,
        SETTING_GAME,
        PREPARING_GAME,
        SELECTING_PIECE, 
        SELECTING_PLACE, 
        DOING_PLAYER_TURN,
        TURN_CHANGE, 
        IN_CONVERSATION, 
        TURN_FINISHED, 
        AI_TURN
    }
    public GameState state;
    private GameState beforeState;

    public enum TurnState
    {
        PLAYER_TURN,
        OPPONENT_TURN
    }

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
            if(turnRemainUI != null)
                turnRemainUI.text = trunRemain.ToString();
        }
    }    
    [SerializeField]
    private TextMeshProUGUI turnRemainUI;

    private void Start()
    {
        TurnRemain = 10;
        state = GameState.SELECTING_PIECE;
        

    }


    public void GameFlow()
    {
        // 시작 이벤트 실행


        // 게임 실행

        // 기물 배치 단계

        // 턴 차례대로 진행



    }


    public void GameStateUpdate()
    {
        switch(state)
        {
            
            case GameState.START:
                DialogueManager.Instance.StartDialogue();
                ChangeGameState(GameState.SETTING_GAME);
                break; 

            case GameState.SETTING_GAME:
                gameSetter.SetPlayers(0);
                break;
            
            case GameState.PREPARING_GAME:
                break;

            case GameState.SELECTING_PIECE:
                break;

            case GameState.SELECTING_PLACE:
                break;
                
            case GameState.DOING_PLAYER_TURN:
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

    public void ChangeGameState(GameState nextState)
    {
        beforeState = state;
        state = nextState;
    }

    public void GoBackGameState()
    {
        state = beforeState;
    }

}
