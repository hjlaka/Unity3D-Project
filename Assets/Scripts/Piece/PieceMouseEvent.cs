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
        piece.ChangeColorTempBack();
    }

    private void OnMouseUpAsButton()
    {

        // �⹰ ���� �ܰ迡�� �⹰ ����
        // �⹰ ���� ��
        //      1. �⹰ ���
        //      2. �⹰ ����
        //      3. �⹰ ����
        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
        {
            if (GameManager.Instance.curPlayer != piece.Belong)
            {
                Debug.Log("�⹰ ���� ���� �ƴ�. ��: " + GameManager.Instance.curPlayer + "/ �⹰ ����: " + piece.Belong);
                return;
            }
            if (piece.Belong is AI)
            {
                Debug.Log("ai�� �⹰�� ������ �� ����");
                return;
            }
            PlaceManager.Instance.SelectPiece(piece);
        }

        else if (GameManager.Instance.state == GameManager.GameState.SELECTING_PLACE)
        {
            // �ڽ��̶��
            if (PlaceManager.Instance.SelectedPiece == piece)
            {
                Debug.Log("�ڽ� Ŭ��");
                PlaceManager.Instance.CancleSelectPiece();
            }
                

            // ���� �� �⹰�̶��
            else if (piece.team.TeamId == PlaceManager.Instance.SelectedPiece.team.TeamId)
            {
                Debug.Log("���� �� Ŭ��");
                PlaceManager.Instance.CancleSelectPiece();
                PlaceManager.Instance.SelectPiece(piece);
            }
            // �ٸ� �� �⹰�̶��
            else
            {
                // ������ �� �ִٸ�
                if (piece.place.IsAttackableByCurPiece)
                {
                    //���� 
                    // ���õ� �⹰�� ������
                    // (������ �Լ� ���ο��� ���� ���� ����)
                    ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.ATTACK, PlaceManager.Instance.SelectedPiece, piece));
                    ChessEventManager.Instance.GetEvent();
                    DialogueManager.Instance.CheckDialogueEvent();
                    PlaceManager.Instance.MoveProcess(PlaceManager.Instance.SelectedPiece, piece.place);

                }
            }

        }
    }
}
