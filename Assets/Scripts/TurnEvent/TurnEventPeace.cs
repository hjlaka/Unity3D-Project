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
        ChessEventManager.Instance.GetEvent();
        DialogueManager.Instance.CheckDialogueEvent(NextStep);
    }
}
