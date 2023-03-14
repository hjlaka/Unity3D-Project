using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnEventAttack : TurnEvent
{
    [Header("Attack")]
    [SerializeField]
    private Piece target; //Unit 클래스로 변경 고려
    public UnityEvent OnTargetAttack;
    public UnityEvent OnTargetAttackEnd;



    protected override void PreTurn()
    {
        base.PreTurn();

        target = targetable as Piece;

        if(null == target)
        {
            Debug.LogError("공격 대상 기물 들어오지 않음");
        }
    }

    protected override IEnumerator HandleDeclare()
    {

        // 선언
        // 대사 선정? (PreTurn 단계에서 미리 선정?)
        DialogueManager.Instance.AddDialogue(new DialogueManager.DialogueUnit(subject, "공격하겠다"));
        DialogueManager.Instance.AddDialogue(new DialogueManager.DialogueUnit(target, "나를!?"));
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
        Debug.Log(string.Format("======={0}이가 {1}을 공격======", subject, target));

        yield return new WaitForSeconds(2f);

        NextStep();
    }


    protected override IEnumerator HandleResult()
    {
        yield return null;
        // 후처리 대화 이벤트 발동. (옵저버 계산 후에 이루어져야 할 일이다.)
        Debug.Log("결과들");
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
