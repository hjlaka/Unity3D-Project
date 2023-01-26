using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingleTon<GameManager>
{
    public UnityEvent OnTest;
    public enum GameState 
    { 
        START_GAME,
        SELECTING_AI,
        SETTING_GAME,
        PREPARING_GAME,
        PREPARING_GAME_ON,
        PREPARING_GAME_END,
        SELECTING_PIECE, 
        SELECTING_PLACE, 
        DOING_PLAYER_TURN_START,
        DOING_PLAYER_TURN,
        TURN_FINISHED,
        TURN_CHANGE, 
        IN_CONVERSATION,
        OPPONENT_TURN_START,
        OPPONENT_TURN,
        RETURN,
        GAME_END,
        OUT_OF_GAME
    }

    GameStateMachine gameState;


    public enum TurnState
    {
        BOTTOM_TURN,
        TOP_TURN,
        RETURN
    }

    public enum PlayerType
    {
        PLAYER,
        PLAYER2,
        AI
    }



    [Header("EngineSetting")]
    [SerializeField]
    public GameSetter gameSetter;

    [SerializeField]
    public AI aiManager;

    [SerializeField]
    public PlayerSetter playerSetter;

    [SerializeField]
    private TextMeshProUGUI turnRemainUI;

    public UnityEvent OnTurnFinished;

    [Header("GameSetting")]
    [SerializeField]
    public TeamData.Direction playerTeamDirection;

    [SerializeField]
    public PlayerType playerType;
    [SerializeField]
    public PlayerType opponentType;


    [SerializeField]
    public Player player;

    public Player opponentPlayer;

    public Player topPlayer;
    public Player bottomPlayer;


    public Player Player { get { return player; } }
    public Player OpponentPlayer { get { return opponentPlayer; } }

    [Header("HomeSetting")]
    public Transform topHome;
    public Transform bottomHome;

    [Header("RunningGame")]
    public GameState state;
    private GameState beforeState;
    public TurnState turnState;

    public Player curPlayer;

    public UnityEvent OnOutOfGame;

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
        state = GameState.OUT_OF_GAME;
        turnState = TurnState.BOTTOM_TURN;
        OnOutOfGame?.Invoke();

    }

    private void Update()
    {
        GameStateUpdate();
    }

    public void GameStateUpdate()
    {
        switch(state)
        {
            case GameState.SELECTING_AI:
                break;
            case GameState.START_GAME:
                OnTest?.Invoke();
                ApplyPlayerType();
                ApplyOpponentType();
                ApplyBothPlayerDirection();
                curPlayer = bottomPlayer;
                
                ChangeGameState(GameState.SETTING_GAME);
                break; 

            case GameState.SETTING_GAME:
                // ��ȭ�� �ִٸ� ��ȭ ���� ����
                DialogueManager.Instance.CheckDialogueEvent();

                // ��ȭ�� ���̻� ���ٸ� ��� ����
                if (state != GameState.SETTING_GAME) break;
                    
                gameSetter.SetTopTeam(0);
                //gameSetter.SetSettingEvent(0);
                

                ChangeGameState(GameState.PREPARING_GAME);
                break;
            
            case GameState.PREPARING_GAME:
                DialogueManager.Instance.CheckDialogueEvent();

                // ��ȭ�� ���̻� ���ٸ� ��� ����
                if (state != GameState.PREPARING_GAME) break;

                gameSetter.SetBottomTeam(0);

                PlayerDataManager.Instance.EnablePlayerListUI(); // �ѹ� �ϰ� �Ѿ�� �Ѵ�.
                // �Ϸ� ��ư�� ������ ��, �������� �Ѿ��. ���� �Է��� ��� �ؾ� �Ѵ�.// �ܺο��� �ٲ� ���ۿ� ���°�? Ȥ�� ��ȣ�� �޴� �� ������?


                ChangeGameState(GameState.PREPARING_GAME_ON);
                
                break;

            case GameState.PREPARING_GAME_ON:
                playerSetter.GetBoard();
                playerSetter.MakeBoardSetable();
                Debug.Log("�غ� �ܰ� ����");

                break;

            case GameState.PREPARING_GAME_END:
                playerSetter.MakeBoardSetableNot();
                ChangeGameState(GameState.SELECTING_PIECE);
                PlayerDataManager.Instance.DisablePlayerListUI();
                player.ShoutOnGame();
                DialogueManager.Instance.CheckDialogueEvent();
                break;


            case GameState.SELECTING_PIECE:
                break;

            case GameState.SELECTING_PLACE:
                break;

            case GameState.DOING_PLAYER_TURN_START:
                ChangeGameState(GameState.DOING_PLAYER_TURN);
                Debug.Log("�÷��̾���");
                ChessEventManager.Instance.GetEvent();
                DialogueManager.Instance.CheckDialogueEvent();
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
                // �̺�Ʈ ���� ��ȣ�� �޾ƿ�
                Debug.Log("���� ���� �˻�");
                if (IsEnded())
                {
                    Debug.Log("���� ���� �ν�");
                    ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.GAME_END, null, null));
                    ChangeGameState(GameState.GAME_END);
                }   
                // �̺�Ʈ ����
                // ü�� �̺�Ʈ
                ChessEventManager.Instance.GetEvent();

                // ��ȭ �̺�Ʈ
                DialogueManager.Instance.CheckDialogueEvent();

                

                // ��ȭ�� ���̻� ���ٸ� ��� ����
                if (state == GameState.TURN_FINISHED)
                {
                    OnTurnFinished?.Invoke();
                    ChangeGameState(GameState.TURN_CHANGE);
                }
                    
                break;

            case GameState.OPPONENT_TURN_START:
                ChangeGameState(GameState.OPPONENT_TURN);
                Debug.Log("�����");
                ChessEventManager.Instance.GetEvent();
                DialogueManager.Instance.CheckDialogueEvent();
                break;

            case GameState.OPPONENT_TURN:
                break;

            case GameState.RETURN:
                // �ǵ����� �۾� ���� �� ���ƿ��� �� �Ʒ� ����
                ChangeGameState(GameState.TURN_CHANGE);
                break;

            case GameState.GAME_END:
                topPlayer.GoToHome();
                bottomPlayer.GoToHome();
                ChangeGameState(GameState.OUT_OF_GAME);
                break;

            case GameState.OUT_OF_GAME:
                break;

            default: 
                state = GameState.TURN_CHANGE; 
                break;
        }
    }
    private void ApplyPlayerType()
    {
        switch (playerType)
        {
            case PlayerType.AI:
                GameObject playerObj = player.gameObject;
                Destroy(player);
                playerObj.AddComponent<AI>();
                player = playerObj.GetComponent<AI>();
                break;
            case PlayerType.PLAYER:
                player = Player;
                break;
            case PlayerType.PLAYER2:
                GameObject player2Player = new GameObject();
                player2Player.AddComponent<Player>();
                player = player2Player.GetComponent<Player>();
                break;
        }

    }
    private void ApplyOpponentType()
    {
        switch (opponentType)
        {
            case PlayerType.AI:
                opponentPlayer = aiManager;
                break;
            case PlayerType.PLAYER:
                opponentPlayer = Player;
                break;
            case PlayerType.PLAYER2:
                GameObject player2Opponent = new GameObject();
                player2Opponent.AddComponent<Player>();
                opponentPlayer = player2Opponent.GetComponent<Player>();
                break;
        }

    }

    private void ApplyBothPlayerDirection()
    {
        // ������� ������ ������ ������ ��.
        switch (playerTeamDirection)
        {
            case TeamData.Direction.UpToDown:
                //player.SetTeamTurn(TurnState.TOP_TURN);
                //opponentPlayer.SetTeamTurn(TurnState.BOTTOM_TURN);

                topPlayer = player;
                bottomPlayer = opponentPlayer;
                break;

            case TeamData.Direction.DownToUp:
                //player.SetTeamTurn(TurnState.BOTTOM_TURN);
                //opponentPlayer.SetTeamTurn(TurnState.TOP_TURN);

                topPlayer = opponentPlayer;
                bottomPlayer = player;
                break;
        }

        topPlayer.homeLocation = topHome;
        bottomPlayer.homeLocation = bottomHome;
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
            curPlayer = topPlayer;
            if(curPlayer is AI)
            {
                ((AI)opponentPlayer).DoTurn();
                
            }
            ChangeGameState(GameState.SELECTING_PIECE);
            // ���⼭ ����? - ���� ��� �����?
        }
        else if (turnState == TurnState.TOP_TURN)
        {
            turnState = TurnState.BOTTOM_TURN;
            curPlayer = bottomPlayer;
            if (curPlayer is AI)
            {
                ((AI)player).DoTurn();
            }
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

    private bool IsEnded()
    {
        if (player.CoreUnit != null && !player.CoreUnit.IsOnGame)
            return true;
        else if (opponentPlayer.CoreUnit != null && !opponentPlayer.CoreUnit.IsOnGame)
            return true;
        else
            return false;
    }

    public void GameEnd()
    {

    }

    private void ChangeGameStateMachine(GameStateMachine nextGameState)
    {
        gameState.StateExit();
        gameState = nextGameState;
        gameState.StateEnter();
    }

    private void GetGameStateInstance(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.START_GAME:
                return;
            default:
                return;
        }
    }
}
