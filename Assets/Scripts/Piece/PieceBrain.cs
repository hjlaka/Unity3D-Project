using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBrain : MonoBehaviour
{
    private Piece piece;

    private void Awake()
    {
        piece = GetComponent<Piece>();
    }

    public void PlaceToDesire(Place targetPlace, DecidedStateLists recognized)
    {

        /*if (recognized.movable.Count <= 0)
        {
            Debug.Log("�̵��� �� �ִ� ���� ����");
            PlaceManager.Instance.CancleSelectPiece();
            return;
        }
        if (targetPlace != null)
        {
            if (targetPlace.Piece != null)
            {
                Debug.Log("��������!");
                //AttackTo(targetPlace);
                ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.ATTACK, piece, targetPlace.Piece));
                ChessEventManager.Instance.GetEvent();
                DialogueManager.Instance.CheckDialogueEvent();
                PlaceManager.Instance.MoveProcess(piece, targetPlace);
            }
            else
            {
                Debug.Log("�̵�����!");
                PlaceManager.Instance.MoveProcess(piece, targetPlace);
            }
        }
        else
        {
            Debug.Log("�����̰� ���� ���� ����");
            PlaceManager.Instance.CancleSelectPiece();
        }*/
    }
}
