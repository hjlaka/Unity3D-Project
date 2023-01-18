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
        AI_TURN,
        RETURN,
        GAME_END
    }
    public GameState state;
    private GameState beforeState;

    public enum TurnState
    {
        PLAYER_TURN,
        OPPONENT_TURN,
        RETURN
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

    [Header("DebugMode")]
    public bool scoreDebugMode;

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
                // 대화가 있다면 대화 상태 진입
                DialogueManager.Instance.CheckDialogueEvent();

                //if(DialogueManager.Instance.IsDialogueExist())
                    // 대화 상태 진입

                // 대화가 더이상 없다면 계속 진행
                if(state == GameState.START)
                    ChangeGameState(GameState.SETTING_GAME);
                break; 

            case GameState.SETTING_GAME:
                gameSetter.SetOpponents(0);
                
                ChangeGameState(GameState.PREPARING_GAME);
                break;
            
            case GameState.PREPARING_GAME:
                gameSetter.SetPlayers(0);
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

            case GameState.AI_TURN:
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

        if (turnState == TurnState.PLAYER_TURN)
        {
            turnState = TurnState.OPPONENT_TURN;
            ChangeGameState(GameState.AI_TURN);
            aiManager.AITurn();         // 여기서 시작? - 상태 기계 만들기?
        }
        else if (turnState == TurnState.OPPONENT_TURN)
        {
            turnState = TurnState.PLAYER_TURN;
            ChangeGameState(GameState.SELECTING_PIECE);
            
        }
        else if (turnState == TurnState.RETURN)
        {
            turnState = TurnState.PLAYER_TURN;
            ChangeGameState(GameState.SELECTING_PIECE);

        }
    }

    public void ChangeTurn(TurnState turn)
    {
        turnState = turn;
    }

}
