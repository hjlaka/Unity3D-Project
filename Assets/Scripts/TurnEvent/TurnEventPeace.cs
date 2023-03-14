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

        // 위치 확보
        targetPlace = targetable as Place;

        if(null == targetPlace)
        {
            Debug.LogError("위치가 들어오지 않음");
        }
    }
    protected override IEnumerator HandleDeclare()
    {
        Debug.Log("선언");

        // 단순한 이동에 매번 대사가 나오면 피로할 것임

        yield return null;

        NextStep();
    }
    protected override IEnumerator HandleReactToDeclare()
    {
        Debug.Log("선언에 반응");

        // 중요한 자리라면 주변 기물들이 반응?

        yield return null;

        NextStep();
    }

    protected override IEnumerator HandleMove()
    {
        Debug.Log("움직임 대기 0.5초");
        // 목적지 설정

        //subject.SetMoveTarget(targetPlace.GetPosition());
        //subject.MoveToTarget();

        // 지정한 위치까지 움직임



        yield return new WaitForSeconds(0.5f);
        NextStep();
    }
    protected override IEnumerator HandleEngage()
    {
        Debug.Log("엮임");

        // 목적지 도착과 그 후 연산

        PlaceManager.Instance.MovePiece(subject, targetPlace);

        yield return null;

        NextStep();
    }
    protected override IEnumerator HandleResult()
    {
        Debug.Log("결과");
        yield return null;
        DialogueManager.Instance.CheckDialogueEvent(NextStep);
    }
}
