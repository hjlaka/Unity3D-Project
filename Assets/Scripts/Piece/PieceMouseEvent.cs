using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Piece))]
public class PieceMouseEvent : MonoBehaviour
{

    [SerializeField]
    private Color mouseOver;

    private Piece piece;

    private void Awake()
    {
        piece = GetComponent<Piece>();
    }
    private void OnMouseOver()
    {
        piece.ChangeColorTemp(mouseOver);
    }

    private void OnMouseExit()
    {
        Debug.Log("���콺 ��");
        piece.ChangeColorTempBack();
    }

    private void OnMouseUpAsButton()
    {

        // �⹰ ���� �ܰ迡�� �⹰ ����
        // �⹰ ���� ��
        //      1. �⹰ ���
        //      2. �⹰ ����
        //      3. �⹰ ����

        // ���� üũ
        if (GameManager.Instance.CurStateType != GameManager.GameState.ON_TURN)
        {
            Debug.Log("�� �ܰ谡 �ƴ�");
            return;
        }

        if (GameManager.Instance.curPlayer is AI)
        {
            Debug.Log("AI�� ����");
            return;
        }  
        
        if (PlaceManager.Instance.SelectedPiece == null)    // ���õ� �⹰�� �ִ��� Ȯ��? Ȥ�� ���� Ȯ��?
        {
            Debug.Log("�⹰ ������");
            SelectPiece(piece);
        }
        else
        {
            //���õ� �⹰�� �ִ� ���¿��� �ٽ� Ŭ��
            // �ڽ��̶��
            if (PlaceManager.Instance.SelectedPiece == piece)
            {
                CancelSelect();
            }

            // ���� �� �⹰�̶��
            else if (piece.IsSameTeam(PlaceManager.Instance.SelectedPiece)) 
            {
                Debug.Log("���� �� Ŭ��");
                CancelSelect();
                SelectPiece(piece);
            }
            // �ٸ� �� �⹰�̶��
            else
            {
                // ������ �� �ִٸ�
                if (piece.place.IsAttackableByCurPiece)
                {
                    //���� 
                    // �Ŵ������� ������ �� �������� �˷���.

                    Attack();
                }
            }
        }
    }

    private void SelectPiece(Piece piece)
    {
        if (piece.Belong != GameManager.Instance.curPlayer)
        {
            Debug.Log("�⹰ ���� ���� �ƴ�. ��: " + GameManager.Instance.curPlayer + "/ �⹰ ����: " + piece.Belong);
            return;
        }
        PlaceManager.Instance.SelectPiece(piece);
    }

    private void CancelSelect()
    {
        Debug.Log("�ڽ� Ŭ��");
        PlaceManager.Instance.CancleSelectPiece();
    }

    private void Attack()
    {
        Debug.Log("����");
        /*ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.ATTACK, PlaceManager.Instance.SelectedPiece, piece));
        ChessEventManager.Instance.GetEvent();
        DialogueManager.Instance.CheckDialogueEvent();
        PlaceManager.Instance.MoveProcess(PlaceManager.Instance.SelectedPiece, piece.place);*/
        PlaceManager.Instance.GetWill(PlaceManager.Instance.SelectedPiece, piece);
    }
}
