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
        ChessEventManager.Instance.GetEvent();
        DialogueManager.Instance.CheckDialogueEvent(NextStep);
    }
}
