using GameState;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingleTon<GameManager>
{
    public UnityEvent OnTest;
    public enum GameState 
    { 
        NONE,
        START_GAME,
        SELECTING_AI,
        SETTING_GAME,
        PREPARING_GAME,
        SELECTING_PIECE, 
        SELECTING_PLACE, 
        DOING_PLAYER_TURN_START,
        DOING_PLAYER_TURN,
        ACTION,
        TURN_FINISHED,
        TURN_CHANGE, 
        IN_CONVERSATION,
        OPPONENT_TURN_START,
        OPPONENT_TURN,
        RETURN,
        GAME_END,
        OUT_OF_GAME,
        ON_TURN
    }


    // =========================== StateMachine ================================
    public StateBehaviour<GameManager> curState { get; private set; }
    [SerializeField]
    private GameState curStateType;
    private GameState nextStateType;
    public GameState CurStateType { get { return curStateType; } private set { curStateType = value; } }

    public StateBehaviour<GameManager> stateStart { get; private set; }
    public StateBehaviour<GameManager> stateOnTurn { get; private set; }
    public StateBehaviour<GameManager> statePreparing { get; private set; }
    public StateBehaviour<GameManager> stateSetting { get; private set; }
    public StateBehaviour<GameManager> stateFinishTurn { get; private set; }

    private Dictionary<GameState, StateBehaviour<GameManager>> stateDic;

    // ==========================================================================


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



    // ��� ���� ����
    public bool playerValidToSelectPiece { get; set; }
    public bool playerValidToSelectPlace { get; set; }






    [Header("DebugMode")]
    public bool scoreDebugMode;


    private readonly StringBuilder debugLog1 = new StringBuilder();

    private void Awake()
    {
        stateDic = new Dictionary<GameState, StateBehaviour<GameManager>>();

        stateStart = GetComponent<StateGameStart>();
        stateSetting = GetComponent<StateSettingGame>();
        statePreparing = GetComponent<StatePreparingGame>();
        stateOnTurn = GetComponent<StateOnTurn>();
        stateFinishTurn = GetComponent<StateTurnFinished>();

        stateDic.Add(GameState.START_GAME, stateStart);
        stateDic.Add(GameState.SETTING_GAME, stateSetting);
        stateDic.Add(GameState.PREPARING_GAME, statePreparing);
        stateDic.Add(GameState.ON_TURN, stateOnTurn);
        stateDic.Add(GameState.TURN_FINISHED, stateFinishTurn);


        curState = null;
        curStateType = GameState.NONE;
        nextStateType = GameState.OUT_OF_GAME;
    }

    private void Start()
    {
        TurnRemain = 10;
        state = GameState.OUT_OF_GAME;
        turnState = TurnState.BOTTOM_TURN;
        OnOutOfGame?.Invoke();

    }

    private void Update()
    {
        //�ӽ�
        ChangeGameStateMachine();
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
                topPlayer = player;
                bottomPlayer = opponentPlayer;
                break;

            case TeamData.Direction.DownToUp:
                topPlayer = opponentPlayer;
                bottomPlayer = player;
                break;
        }

        topPlayer.homeLocation = topHome;
        bottomPlayer.homeLocation = bottomHome;
    }

/*    public void ChangeGameState(GameState nextState)
    {
        Debug.Log(string.Format("���� ���� ����: {0} ���� {1}��.", state, nextState));
        beforeState = state;
        state = nextState;
    }*/

    public void GoBackGameState()
    {
        Debug.Log("���� �� �������� ����: " + beforeState);
        state = beforeState;
    }

    public void ChangeTurn()
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
            //ChangeGameState(GameState.SELECTING_PIECE);
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
            //ChangeGameState(GameState.SELECTING_PIECE);

        }
        else if (turnState == TurnState.RETURN)
        {
            turnState = TurnState.BOTTOM_TURN;
            //ChangeGameState(GameState.SELECTING_PIECE);
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

    public void SetNextState(GameState stateType)
    {
        nextStateType = stateType;

    }
    public void ChangeGameStateMachine()
    {
        if (nextStateType == curStateType)
            return;

        // --------- ����� -----------
        debugLog1.Clear();
        debugLog1.Append("���� ���� ���� ����: ");
        debugLog1.Append(nextStateType.ToString());
        Debug.Log(debugLog1);
        // ----------------------------

        StateBehaviour<GameManager> nextState;
        if (stateDic.TryGetValue(nextStateType, out nextState))
        {
            curState?.StateExit();
            curState = nextState;
            curStateType = nextStateType;
            curState?.StateEnter();
            Debug.Log("���� ���� ����");
        }
        else
        {
            Debug.Log("�ش� ���� Ŭ���� ����");
        }

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
