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
        ON_PEACE,
        ON_TURN
    }

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


    // =========================== StateMachine ================================

    public StateBehaviour<GameManager> curState { get; private set; }
    [SerializeField]
    private GameState curStateType;
    [SerializeField]
    private GameState nextStateType;
    public GameState CurStateType { get { return curStateType; } private set { curStateType = value; } }
    public GameState NextStateType { get { return nextStateType; } private set { nextStateType = value; } }

    public StateBehaviour<GameManager> statePeace { get; private set; }
    public StateBehaviour<GameManager> stateStart { get; private set; }
    public StateBehaviour<GameManager> stateOnTurn { get; private set; }
    public StateBehaviour<GameManager> statePreparing { get; private set; }
    public StateBehaviour<GameManager> stateSetting { get; private set; }
    public StateBehaviour<GameManager> stateFinishTurn { get; private set; }
    public StateBehaviour<GameManager> stateGameEnd { get; private set; }

    private Dictionary<GameState, StateBehaviour<GameManager>> stateDic;

    //public UnityAction<GameState> OnGameStateChanged; 

    // ==========================================================================


    [Header("EngineSetting")]
    [SerializeField]
    public GameSetter gameSetter;

    [SerializeField]
    public AI aiManager;

    [SerializeField]
    public PlayerSetter playerSetter;

    [SerializeField]
    private TextMeshProUGUI turnRemainUI;

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


    // 사용 예정 변수
    public bool playerValidToSelectPiece { get; set; }
    public bool playerValidToSelectPlace { get; set; }


    [Header("DebugMode")]
    public bool scoreDebugMode;

    public UnityEvent OnOutOfGame;


    private readonly StringBuilder debugLog1 = new StringBuilder();

    private void Awake()
    {
        stateDic = new Dictionary<GameState, StateBehaviour<GameManager>>();

        statePeace = GetComponentInChildren<StatePeace>();
        stateStart = GetComponentInChildren<StateGameStart>();
        stateSetting = GetComponentInChildren<StateSettingGame>();
        statePreparing = GetComponentInChildren<StatePreparingGame>();
        stateOnTurn = GetComponentInChildren<StateOnTurn>();
        stateFinishTurn = GetComponentInChildren<StateTurnFinished>();
        stateGameEnd = GetComponentInChildren<StateGameEnd>();

        stateDic.Add(GameState.ON_PEACE, statePeace);
        stateDic.Add(GameState.START_GAME, stateStart);
        stateDic.Add(GameState.SETTING_GAME, stateSetting);
        stateDic.Add(GameState.PREPARING_GAME, statePreparing);
        stateDic.Add(GameState.ON_TURN, stateOnTurn);
        stateDic.Add(GameState.TURN_FINISHED, stateFinishTurn);
        stateDic.Add(GameState.GAME_END, stateGameEnd);

        curState = statePeace;
        curStateType = GameState.NONE;
        nextStateType = GameState.ON_PEACE;
        UpdateGameStateMachine();
    }

    private void Start()
    {
        TurnRemain = 10;
        state = GameState.ON_PEACE;
        turnState = TurnState.BOTTOM_TURN;
        OnOutOfGame?.Invoke();
    }

    private void Update()
    {
        //머신
        curState?.Handle();
    }

    public void GoBackGameState()
    {
        Debug.Log("게임 씬 이전으로 변경: " + beforeState);
        state = beforeState;
    }

    public void ChangeTurn()
    {
        //AI와 플레이어 턴은 한번씩만 진행되는가?

        switch(turnState)
        {
            case TurnState.BOTTOM_TURN:
                turnState = TurnState.TOP_TURN;
                curPlayer = topPlayer;
                if (curPlayer is AI)
                {
                    ((AI)opponentPlayer).DoTurn();
                }
                break;
            case TurnState.TOP_TURN:
                turnState = TurnState.BOTTOM_TURN;
                curPlayer = bottomPlayer;
                if (curPlayer is AI)
                {
                    ((AI)player).DoTurn();
                }
                break;
            case TurnState.RETURN:
                turnState = TurnState.BOTTOM_TURN;
                break;
        }
    }

    public void ChangeTurn(TurnState turn)
    {
        turnState = turn;
    }


    private void JudgeGame()
    {
        // 현재 플레이어가 포기 신호를 보내면 게임 종료.

        // 상대편 왕이 없거나 체크메이트 상태면 게임 종료.

        // 상대편 이동할 기물이 없으면 무승부.
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

    public void SetNextState(GameState stateType)
    {
        nextStateType = stateType;
        Debug.Log("게임 상태 변경 예약");

    }
    public void UpdateGameStateMachine()
    {
        if (nextStateType == curStateType)
            return;

        // --------- 디버그 -----------
        debugLog1.Clear();
        debugLog1.Append("게임 상태 변경 감지: ");
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
            Debug.Log("게임 상태 변경");
        }
        else
        {
            Debug.Log("해당 상태 클래스 없음");
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
