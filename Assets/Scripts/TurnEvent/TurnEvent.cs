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
        //�غ�
        //�� ��Ȳ ���.
    }

    private IEnumerator TurnCoroutine()
    {
        // ��ü �ڷ�ƾ�� ����
        // �� �ȿ��� ���� ��Ȳ �ڷ�ƾ�� ������ �����.
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
                Debug.Log("����");
                //�Ѹ�? �ƴϸ� ������ ���̵� ��ΰ� ��?
                break;

            case Step.ReactToDeclare:
                Debug.Log("���� ����");
                break;

            case Step.Move:
                Debug.Log("������");
                break;

            case Step.Engage:
                Debug.Log("����");
                // ���, ����, �ٽ� ��� (�� ����ȭ �� �� ����)
                break;

            case Step.Result:
                Debug.Log("���");
                // ����
                break;

            case Step.Record:
                // �޸��� �� ���? �Ŵ��� ȣ��?
                break;
        }
    }

    private IEnumerator Declare()
    {
        Debug.Log("����");
        yield return null;

    }

    private void NextStep()
    {
        step++;
        StepTurn();
    }
}
