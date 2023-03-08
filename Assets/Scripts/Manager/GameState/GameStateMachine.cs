using GameState;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    // 컴포넌트가 길어질 것 같아 따로 게임 오브젝트를 만들려고 했지만,
    // 상태 머신 내부에서 게임매니저를 건드린다.
    // 그러나 상태 자체를 바꿀 때는, 상태 머신이 있는 이 스크립트에 접근해야 한다.
    // 상태 머신 내부에서 T가 아닌, 싱글턴을 통해 게임 매니저를 건드린다면 게임 오브젝트를 따로 빼는 것이 유효하겠다.
    // 싱글턴 사용? 아니면... 게임 매니저에 상태 머신 두기?


    StateBehaviour<GameManager> curState;

    StateBehaviour<GameManager> stateStart;
    StateBehaviour<GameManager> stateOnTurn;
    StateBehaviour<GameManager> statePreparing;
    StateBehaviour<GameManager> stateSetting;
    StateBehaviour<GameManager> stateFinishTurn;

    private void Awake()
    {
        stateStart = GetComponent<StateGameStart>();
        stateOnTurn = GetComponent<StateOnTurn>();
        statePreparing = GetComponent<StatePreparingGame>();
        stateSetting = GetComponent<StateSettingGame>();
        stateFinishTurn = GetComponent<StateTurnFinished>();

        curState = null;
    }

    private void Start()
    {
        ChangeGameStateMachine(stateStart);
    }

    private void ChangeGameStateMachine(StateBehaviour<GameManager> nextGameState)
    {
        // --------- 디버그 -----------
        StringBuilder debugLog1 = new StringBuilder();
        debugLog1.Append("게임 상태 변경: ");
        debugLog1.Append(nextGameState);
        Debug.Log(debugLog1);
        // ----------------------------

        curState?.StateExit();
        curState = nextGameState;
        curState?.StateEnter();
    }
}
