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
        TURN_FINISHED,
        TURN_CHANGE, 
        IN_CONVERSATION, 
        OPPONENT_TURN,
        RETURN,
        GAME_END
    }


    public enum TurnState
    {
        BOTTOM_TURN,
        TOP_TURN,
        RETURN
    }

    public enum OpponentType
    {
        PLAYER2,
        AI
    }



    [Header("EngineSetting")]
    [SerializeField]
    private GameSetter gameSetter;

    [SerializeField]
    private AI aiManager;

    [SerializeField]
    private TextMeshProUGUI turnRemainUI;

    [Header("GameSetting")]
    [SerializeField]
    private TeamData.Direction playerTeamDirection;

    [SerializeField]
    private OpponentType opponentType;


    [SerializeField]
    private Player player;

    private Player opponentPlayer;

    public Player Player { get { return player; } }
    public Player OpponentPlayer { get { return opponentPlayer; } }

    [Header("RunningGame")]
    public GameState state;
    private GameState beforeState;
    public TurnState turnState;

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


    [Header("DebugMode")]
    public bool scoreDebugMode;

    private void Start()
    {
        TurnRemain = 10;
        //state = GameState.SELECTING_PIECE;
        turnState = TurnState.BOTTOM_TURN;


    }

    private void ApplyOpponentType()
    {
        switch (opponentType)
        {
            case OpponentType.AI:
                opponentPlayer = aiManager;
                break;
            case OpponentType.PLAYER2:
                opponentPlayer = new Player();
                break;
        }

    }

    private void ApplyBothPlayerDirection()
    {
        // ������� ������ ������ ������ ��.
        switch (playerTeamDirection)
        {
            case TeamData.Direction.UpToDown:
                player.SetTeamTurn(TurnState.TOP_TURN);
                opponentPlayer.SetTeamTurn(TurnState.BOTTOM_TURN);
                
                break;

            case TeamData.Direction.DownToUp:
                player.SetTeamTurn(TurnState.BOTTOM_TURN);
                opponentPlayer.SetTeamTurn(TurnState.TOP_TURN);
                break;
        }
    }

    private void Update()
    {
        GameStateUpdate();
    }

    public void GameStateUpdate()
    {
        switch(state)
        {
            case GameState.START:
                ApplyOpponentType();
                ApplyBothPlayerDirection();
                // ��ȭ�� �ִٸ� ��ȭ ���� ����
                DialogueManager.Instance.CheckDialogueEvent();

                //if(DialogueManager.Instance.IsDialogueExist())
                    // ��ȭ ���� ����

                // ��ȭ�� ���̻� ���ٸ� ��� ����
                if(state == GameState.START)
                    ChangeGameState(GameState.SETTING_GAME);
                break; 

            case GameState.SETTING_GAME:
                gameSetter.SetTopTeam(0);
                
                ChangeGameState(GameState.PREPARING_GAME);
                break;
            
            case GameState.PREPARING_GAME:
                gameSetter.SetBottomTeam(0);
                PlayerDataManager.Instance.EnablePlayerListUI(); // �ѹ� �ϰ� �Ѿ�� �Ѵ�.
                // �Ϸ� ��ư�� ������ ��, �������� �Ѿ��. ���� �Է��� ��� �ؾ� �Ѵ�.// �ܺο��� �ٲ� ���ۿ� ���°�? Ȥ�� ��ȣ�� �޴� �� ������?
                ChangeGameState(GameState.SELECTING_PIECE);
                PlayerDataManager.Instance.DisablePlayerListUI();
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
                // ���� �κп���
                // ui �����ֱ�
                // ��� �����ϱ�

                // ������Ʈ �κп���
                // Ŭ�� �޾ƿ���?

                // ���� �κп���
                // ���ư���
                break;

            case GameState.TURN_FINISHED:
                // �̺�Ʈ ����
                // ü�� �̺�Ʈ
                ChessEventManager.Instance.GetEvent();

                // ��ȭ �̺�Ʈ
                DialogueManager.Instance.CheckDialogueEvent();

                // �̺�Ʈ ���� ��ȣ�� �޾ƿ�
                // ��ȭ�� ���̻� ���ٸ� ��� ����
                if (state == GameState.TURN_FINISHED)
                    ChangeGameState(GameState.TURN_CHANGE);
                break;

            case GameState.OPPONENT_TURN:
                break;

            case GameState.RETURN:
                // �ǵ����� �۾� ���� �� ���ƿ��� �� �Ʒ� ����
                ChangeGameState(GameState.TURN_CHANGE);
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

        if (turnState == TurnState.BOTTOM_TURN)
        {
            turnState = TurnState.TOP_TURN;
            ChangeGameState(GameState.OPPONENT_TURN);
            if(opponentPlayer is AI)
                ((AI)opponentPlayer).DoTurn();         // ���⼭ ����? - ���� ��� �����?
        }
        else if (turnState == TurnState.TOP_TURN)
        {
            turnState = TurnState.BOTTOM_TURN;
            ChangeGameState(GameState.SELECTING_PIECE);
            
        }
        else if (turnState == TurnState.RETURN)
        {
            turnState = TurnState.BOTTOM_TURN;
            ChangeGameState(GameState.SELECTING_PIECE);

        }
    }

    public void ChangeTurn(TurnState turn)
    {
        turnState = turn;
    }

}
