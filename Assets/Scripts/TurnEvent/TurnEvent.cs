using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnEvent : MonoBehaviour
{
    enum Step
    {
        Declare,
        ReactToDeclare,
        Move,
        Engage,
        Result,
        Record,
        Size
    }
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
        StepTurn();
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

    private void StepTurn()
    {
        switch (step)
        {
            case Step.Declare:

                StartCoroutine(Declare());
                Debug.Log("선언");
                //한명만? 아니면 연관된 아이들 모두가 다?
                break;

            case Step.ReactToDeclare:
                Debug.Log("선언에 반응");
                break;

            case Step.Move:
                Debug.Log("움직임");
                break;

            case Step.Engage:
                Debug.Log("엮임");
                // 대사, 공격, 다시 대사 (더 세분화 될 수 있음)
                break;

            case Step.Result:
                Debug.Log("결과");
                // 퇴장
                break;

            case Step.Record:
                // 메멘토 등 등록? 매니저 호출?
                break;
        }
    }

    private IEnumerator Declare()
    {
        Debug.Log("선언");
        yield return null;

    }

    private void NextStep()
    {
        step++;
        StepTurn();
    }
}
