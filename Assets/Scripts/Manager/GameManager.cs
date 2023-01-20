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
        // 상대편이 결정된 다음에 동작할 것.
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
                // 대화가 있다면 대화 상태 진입
                DialogueManager.Instance.CheckDialogueEvent();

                //if(DialogueManager.Instance.IsDialogueExist())
                    // 대화 상태 진입

                // 대화가 더이상 없다면 계속 진행
                if(state == GameState.START)
                    ChangeGameState(GameState.SETTING_GAME);
                break; 

            case GameState.SETTING_GAME:
                gameSetter.SetTopTeam(0);
                
                ChangeGameState(GameState.PREPARING_GAME);
                break;
            
            case GameState.PREPARING_GAME:
                gameSetter.SetBottomTeam(0);
                PlayerDataManager.Instance.EnablePlayerListUI(); // 한번 하고 넘어가야 한다.
                // 완료 버튼을 눌렀을 시, 다음으로 넘어간다. 무언가 입력을 대기 해야 한다.// 외부에서 바꿀 수밖에 없는가? 혹은 신호를 받는 게 나은가?
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
                // 시작 부분에서
                // ui 보여주기
                // 대사 세팅하기

                // 업데이트 부분에서
                // 클릭 받아오기?

                // 종료 부분에서
                // 돌아가기
                break;

            case GameState.TURN_FINISHED:
                // 이벤트 실행
                // 체스 이벤트
                ChessEventManager.Instance.GetEvent();

                // 대화 이벤트
                DialogueManager.Instance.CheckDialogueEvent();

                // 이벤트 종료 신호를 받아옴
                // 대화가 더이상 없다면 계속 진행
                if (state == GameState.TURN_FINISHED)
                    ChangeGameState(GameState.TURN_CHANGE);
                break;

            case GameState.OPPONENT_TURN:
                break;

            case GameState.RETURN:
                // 되돌리기 작업 종료 후 돌아왔을 때 아래 실행
                ChangeGameState(GameState.TURN_CHANGE);
                break;

            default: 
                state = GameState.TURN_CHANGE; 
                break;
        }
    }

    public void ChangeGameState(GameState nextState)
    {
        Debug.Log("게임 씬 변경: " + nextState);
        beforeState = state;
        state = nextState;
    }

    public void GoBackGameState()
    {
        Debug.Log("게임 씬 이전으로 변경: " + beforeState);
        state = beforeState;
    }

    private void ChangeTurn()
    {
        //AI와 플레이어 턴은 한번씩만 진행되는가?

        if (turnState == TurnState.BOTTOM_TURN)
        {
            turnState = TurnState.TOP_TURN;
            ChangeGameState(GameState.OPPONENT_TURN);
            if(opponentPlayer is AI)
                ((AI)opponentPlayer).DoTurn();         // 여기서 시작? - 상태 기계 만들기?
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
