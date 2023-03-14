using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnEventAttack : TurnEvent
{
    [Header("Attack")]
    [SerializeField]
    private Piece target; //Unit Ŭ������ ���� ���
    public UnityEvent OnTargetAttack;
    public UnityEvent OnTargetAttackEnd;



    protected override void PreTurn()
    {
        base.PreTurn();

        target = targetable as Piece;

        if(null == target)
        {
            Debug.LogError("���� ��� �⹰ ������ ����");
        }
    }

    protected override IEnumerator HandleDeclare()
    {

        // ����
        // ��� ����? (PreTurn �ܰ迡�� �̸� ����?)
        DialogueManager.Instance.AddDialogue(new DialogueManager.DialogueUnit(subject, "�����ϰڴ�"));
        DialogueManager.Instance.AddDialogue(new DialogueManager.DialogueUnit(target, "����!?"));
        DialogueManager.Instance.CheckDialogueEvent(NextStep);

        yield return null;   
    }
    protected override IEnumerator HandleReactToDeclare()
    {
        yield return null;
        NextStep();
    }
    protected override IEnumerator HandleMove()
    {
        yield return null;

        NextStep();
    }

    protected override IEnumerator HandleEngage()
    {
        OnTargetAttack?.Invoke();
        PlaceManager.Instance.MovePiece(subject, target.place);
        Debug.Log(string.Format("======={0}�̰� {1}�� ����======", subject, target));

        yield return new WaitForSeconds(2f);

        NextStep();
    }


    protected override IEnumerator HandleResult()
    {
        yield return null;
        // ��ó�� ��ȭ �̺�Ʈ �ߵ�. (������ ��� �Ŀ� �̷������ �� ���̴�.)
        Debug.Log("�����");
        ChessEventManager.Instance.GetEvent();
        DialogueManager.Instance.CheckDialogueEvent(NextStep);
        
        yield return new WaitForSeconds(1f);
    }


    protected override IEnumerator HandleRecord()
    {
        OnTargetAttackEnd?.Invoke();
        yield return base.HandleRecord();
    }


}
