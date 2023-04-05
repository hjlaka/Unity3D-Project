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
            Debug.Log("이동할 수 있는 곳이 없다");
            PlaceManager.Instance.CancleSelectPiece();
            return;
        }
        if (targetPlace != null)
        {
            if (targetPlace.Piece != null)
            {
                Debug.Log("공격하자!");
                //AttackTo(targetPlace);
                ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.ATTACK, piece, targetPlace.Piece));
                ChessEventManager.Instance.GetEvent();
                DialogueManager.Instance.CheckDialogueEvent();
                PlaceManager.Instance.MoveProcess(piece, targetPlace);
            }
            else
            {
                Debug.Log("이동하자!");
                PlaceManager.Instance.MoveProcess(piece, targetPlace);
            }
        }
        else
        {
            Debug.Log("움직이고 싶은 곳이 없다");
            PlaceManager.Instance.CancleSelectPiece();
        }*/
    }
}
