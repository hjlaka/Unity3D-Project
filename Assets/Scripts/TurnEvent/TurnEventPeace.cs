using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEventPeace : TurnEvent
{
    [Header("Peace")]
    [SerializeField]
    private Place targetPlace;

    protected override void PreTurn()
    {
        base.PreTurn();

        // ��ġ Ȯ��
        targetPlace = targetable as Place;

        if(null == targetPlace)
        {
            Debug.LogError("��ġ�� ������ ����");
        }
    }
    protected override IEnumerator HandleDeclare()
    {
        Debug.Log("����");

        // �ܼ��� �̵��� �Ź� ��簡 ������ �Ƿ��� ����

        yield return null;

        NextStep();
    }
    protected override IEnumerator HandleReactToDeclare()
    {
        Debug.Log("���� ����");

        // �߿��� �ڸ���� �ֺ� �⹰���� ����?

        yield return null;

        NextStep();
    }

    protected override IEnumerator HandleMove()
    {
        Debug.Log("������ ��� 0.5��");
        // ������ ����

        //subject.SetMoveTarget(targetPlace.GetPosition());
        //subject.MoveToTarget();

        // ������ ��ġ���� ������



        yield return new WaitForSeconds(0.5f);
        NextStep();
    }
    protected override IEnumerator HandleEngage()
    {
        Debug.Log("����");

        // ������ ������ �� �� ����

        PlaceManager.Instance.MovePiece(subject, targetPlace);

        yield return null;

        NextStep();
    }
    protected override IEnumerator HandleResult()
    {
        Debug.Log("���");
        yield return null;
        DialogueManager.Instance.CheckDialogueEvent(NextStep);
    }
}
