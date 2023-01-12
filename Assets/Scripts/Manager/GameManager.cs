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
    private TurnState turnState;

    public bool isPlayerTurn;



    [SerializeField]
    private GameSetter gameSetter;

    [SerializeField]
    private AI aiManager;


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
        //state = GameState.SELECTING_PIECE;
        turnState = TurnState.PLAYER_TURN;



    }

    private void Update()
    {
        GameStateUpdate();
    }

    public void GameFlow()
    {
        // ���� �̺�Ʈ ����


        // ���� ����

        // �⹰ ��ġ �ܰ�

        // �� ���ʴ�� ����



    }


    public void GameStateUpdate()
    {
        switch(state)
        {
            
            case GameState.START:
                DialogueManager.Instance.StartDialogue();

                // ��ȭ�� ���̻� ���ٸ� ��� ����
                if(state == GameState.START)
                    ChangeGameState(GameState.SETTING_GAME);
                break; 

            case GameState.SETTING_GAME:
                gameSetter.SetOpponents(0);
                
                ChangeGameState(GameState.PREPARING_GAME);
                break;
            
            case GameState.PREPARING_GAME:
                gameSetter.SetPlayers(0);
                PlayerDataManager.Instance.EnablePlayerListUI(); // �ѹ� �ϰ� �Ѿ�� �Ѵ�.
                ChangeGameState(GameState.SELECTING_PIECE);
                break;

            case GameState.SELECTING_PIECE:
                break;

            case GameState.SELECTING_PLACE:
                break;
                
            case GameState.DOING_PLAYER_TURN:
                break;

            case GameState.TURN_CHANGE:

                ChangeTurn();

                break;

            case GameState.IN_CONVERSATION:
                break;

            case GameState.TURN_FINISHED:
                ChangeGameState(GameState.TURN_CHANGE);
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
        Debug.Log("���� �� ����: " + nextState);
        beforeState = state;
        state = nextState;
    }

    public void GoBackGameState()
    {
        Debug.Log("���� �� �������� ����: " + beforeState);
        state = beforeState;
    }

    private void ChangeTurn()
    {
        //AI�� �÷��̾� ���� �ѹ����� ����Ǵ°�?

        if (turnState == TurnState.PLAYER_TURN)
        {
            turnState = TurnState.OPPONENT_TURN;
            ChangeGameState(GameState.AI_TURN);
            aiManager.AITurn();         // ���⼭ ����? - ���� ��� �����?
        }
        else if (turnState == TurnState.OPPONENT_TURN)
        {
            turnState = TurnState.PLAYER_TURN;
            ChangeGameState(GameState.SELECTING_PIECE);
            
            
        }
            
    }

}
