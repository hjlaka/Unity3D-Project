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

    private void StepTurn(Step nextStep)
    {
        step = nextStep;

        switch (nextStep)
        {
            case Step.Declare:

                StartCoroutine(HandleDeclare());
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

    private IEnumerator HandleDeclare()
    {
        Debug.Log("����");

        // ��󿡰� ��������
        // ���� ���� �Բ� ����Ͽ� ��� ����?

        yield return new WaitForSeconds(3f);

        NextStep();
    }
    private IEnumerator HandleReactToDeclare()
    {
        Debug.Log("���� ����");

        // �������� ����. (�� �������� ���������� �Ͼ�� �� ���� �ִ�)


        yield return new WaitForSeconds(3f);
        NextStep();
    }

    private IEnumerator HandleMove()
    {
        Debug.Log("������");
        // ������ ����
        
        subject.MoveToTarget(target.GetPosition());

        //while()
        //{
        //    yield return null;
        //}
        // ������

        // ������ ��ġ���� ������

        // 


        yield return new WaitForSeconds(3f);
        NextStep();
    }
    private IEnumerator HandleEngage()
    {
        Debug.Log("����");
        yield return new WaitForSeconds(3f);
        NextStep();
    }
    private IEnumerator HandleResult()
    {
        Debug.Log("���");
        yield return new WaitForSeconds(3f);
        NextStep();
    }
    private IEnumerator HandleRecord()
    {
        Debug.Log("���");
        yield return new WaitForSeconds(3f);
        // ��ġ �Ŵ��� ȣ�� Ȥ�� ���� �� ���� ����
        GameManager.Instance.SetNextState(GameManager.GameState.TURN_FINISHED);
    }

    private void NextStep()
    {
        step++;
        StepTurn(step);
    }
}
