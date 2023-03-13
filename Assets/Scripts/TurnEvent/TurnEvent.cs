using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnEvent : MonoBehaviour
{
    public enum Step
    {
        Declare,
        ReactToDeclare,
        Move,
        Engage,
        Result,
        Record,
        Size
    }

    [SerializeField]
    private Step step;
    private Coroutine curStepCoroutine;

    private Piece subject;
    private ITargetable target;

    public void SetTurnEvent(Piece subject, ITargetable target)
    {
        this.subject = subject;
        this.target = target;
    }
    public void DoTurn()
    {
        PreTurn();
        StepTurn(Step.Declare);
    }

    private void PreTurn()
    {
        //준비
        //턴 상황 계산.
    }

    private IEnumerator TurnCoroutine()
    {
        // 전체 코루틴이 돌고
        // 그 안에서 개별 상황 코루틴이 돌도록 만든다.
        yield return null;

        

        for(Step curStep = Step.Declare; curStep < Step.Size; curStep++)
        {

        }

    }

    private void StepTurn(Step nextStep)
    {
        step = nextStep;

        switch (nextStep)
        {
            case Step.Declare:

                StartCoroutine(HandleDeclare());
                Debug.Log("선언");
                //한명만? 아니면 연관된 아이들 모두가 다?
                break;

            case Step.ReactToDeclare:
                StartCoroutine(HandleReactToDeclare());
                Debug.Log("선언에 반응");
                break;

            case Step.Move:
                StartCoroutine(HandleMove());
                Debug.Log("움직임");
                break;

            case Step.Engage:
                StartCoroutine(HandleEngage());
                Debug.Log("엮임");
                // 대사, 공격, 다시 대사 (더 세분화 될 수 있음)
                break;

            case Step.Result:
                StartCoroutine(HandleResult());
                Debug.Log("결과");
                // 퇴장
                break;

            case Step.Record:
                StartCoroutine(HandleRecord());
                // 메멘토 등 등록? 매니저 호출?
                break;
        }
    }

    private IEnumerator HandleDeclare()
    {
        Debug.Log("선언");

        // 대상에게 선전포고
        // 관계 변수 함께 고려하여 대사 선정?

        yield return new WaitForSeconds(3f);

        NextStep();
    }
    private IEnumerator HandleReactToDeclare()
    {
        Debug.Log("선언에 반응");

        // 선전포고에 반응. (앞 순서에서 연쇄적으로 일어나게 할 수도 있다)


        yield return new WaitForSeconds(3f);
        NextStep();
    }

    private IEnumerator HandleMove()
    {
        Debug.Log("움직임");
        // 목적지 설정
        
        subject.MoveToTarget(target.GetPosition());

        //while()
        //{
        //    yield return null;
        //}
        // 움직임

        // 지정한 위치까지 움직임

        // 


        yield return new WaitForSeconds(3f);
        NextStep();
    }
    private IEnumerator HandleEngage()
    {
        Debug.Log("엮임");
        yield return new WaitForSeconds(3f);
        NextStep();
    }
    private IEnumerator HandleResult()
    {
        Debug.Log("결과");
        yield return new WaitForSeconds(3f);
        NextStep();
    }
    private IEnumerator HandleRecord()
    {
        Debug.Log("기록");
        yield return new WaitForSeconds(3f);
        // 배치 매니저 호출 혹은 직접 턴 종료 선언
        GameManager.Instance.SetNextState(GameManager.GameState.TURN_FINISHED);
    }

    private void NextStep()
    {
        step++;
        StepTurn(step);
    }
}
