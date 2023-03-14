using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnEvent : MonoBehaviour
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

    //=============================
    protected Piece subject;
    protected ITargetable targetable;
    //========= Record ============
    protected Place prevPlace;
    //=============================

    

    public void SetTurnEvent(Piece subject, ITargetable targetable)
    {
        this.subject = subject;
        this.targetable = targetable;
    }
    public void DoTurn()
    {
        PreTurn();
        RunStepCoroutine(Step.Declare);
    }

    protected virtual void PreTurn()
    {
        prevPlace = subject.place;
        //준비
        //턴 상황 계산.
    }

    private void RunStepCoroutine(Step nextStep)
    {
        step = nextStep;

        switch (nextStep)
        {
            case Step.Declare:

                curStepCoroutine = StartCoroutine(HandleDeclare());
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

    protected virtual IEnumerator HandleDeclare()
    {
        // 다름.
        yield return null;
        NextStep();
    }

    protected virtual IEnumerator HandleReactToDeclare()
    {
        // 다름.
        yield return null;
        NextStep();
    }

    protected virtual IEnumerator HandleMove()
    {
        // 공통. 움직임
        yield return null;
        NextStep();
    }

    protected virtual IEnumerator HandleEngage()
    {
        // 다름. 엮임

        yield return null;
        NextStep();
    }

    protected virtual IEnumerator HandleResult()
    {
        // 영향력 재계산 등
        yield return null;
        NextStep();
    }

    protected virtual IEnumerator HandleRecord()
    {
        PlaceManager.Instance.SaveMemento(subject, prevPlace, subject.place, targetable as Piece, null);
        PlaceManager.Instance.EndTurn();
        // 공통. 메멘토 기록
        yield return null;
        
    }

    protected virtual void NextStep()
    {
        step++;
        RunStepCoroutine(step);
    }
}
