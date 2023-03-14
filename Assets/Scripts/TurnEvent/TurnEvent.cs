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
        //�غ�
        //�� ��Ȳ ���.
    }

    private void RunStepCoroutine(Step nextStep)
    {
        step = nextStep;

        switch (nextStep)
        {
            case Step.Declare:

                curStepCoroutine = StartCoroutine(HandleDeclare());
                Debug.Log("����");
                //�Ѹ�? �ƴϸ� ������ ���̵� ��ΰ� ��?
                break;

            case Step.ReactToDeclare:
                StartCoroutine(HandleReactToDeclare());
                Debug.Log("���� ����");
                break;

            case Step.Move:
                StartCoroutine(HandleMove());
                Debug.Log("������");
                break;

            case Step.Engage:
                StartCoroutine(HandleEngage());
                Debug.Log("����");
                // ���, ����, �ٽ� ��� (�� ����ȭ �� �� ����)
                break;

            case Step.Result:
                StartCoroutine(HandleResult());
                Debug.Log("���");
                // ����
                break;

            case Step.Record:
                StartCoroutine(HandleRecord());
                // �޸��� �� ���? �Ŵ��� ȣ��?
                break;
        }
    }

    protected virtual IEnumerator HandleDeclare()
    {
        // �ٸ�.
        yield return null;
        NextStep();
    }

    protected virtual IEnumerator HandleReactToDeclare()
    {
        // �ٸ�.
        yield return null;
        NextStep();
    }

    protected virtual IEnumerator HandleMove()
    {
        // ����. ������
        yield return null;
        NextStep();
    }

    protected virtual IEnumerator HandleEngage()
    {
        // �ٸ�. ����

        yield return null;
        NextStep();
    }

    protected virtual IEnumerator HandleResult()
    {
        // ����� ���� ��
        yield return null;
        NextStep();
    }

    protected virtual IEnumerator HandleRecord()
    {
        PlaceManager.Instance.SaveMemento(subject, prevPlace, subject.place, targetable as Piece, null);
        PlaceManager.Instance.EndTurn();
        // ����. �޸��� ���
        yield return null;
        
    }

    protected virtual void NextStep()
    {
        step++;
        RunStepCoroutine(step);
    }
}
