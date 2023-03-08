using GameState;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    // ������Ʈ�� ����� �� ���� ���� ���� ������Ʈ�� ������� ������,
    // ���� �ӽ� ���ο��� ���ӸŴ����� �ǵ帰��.
    // �׷��� ���� ��ü�� �ٲ� ����, ���� �ӽ��� �ִ� �� ��ũ��Ʈ�� �����ؾ� �Ѵ�.
    // ���� �ӽ� ���ο��� T�� �ƴ�, �̱����� ���� ���� �Ŵ����� �ǵ帰�ٸ� ���� ������Ʈ�� ���� ���� ���� ��ȿ�ϰڴ�.
    // �̱��� ���? �ƴϸ�... ���� �Ŵ����� ���� �ӽ� �α�?


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
        // --------- ����� -----------
        StringBuilder debugLog1 = new StringBuilder();
        debugLog1.Append("���� ���� ����: ");
        debugLog1.Append(nextGameState);
        Debug.Log(debugLog1);
        // ----------------------------

        curState?.StateExit();
        curState = nextGameState;
        curState?.StateEnter();
    }
}
