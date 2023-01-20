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
        //Debug.Log(string.Format("{0} ���콺 ����Ŵ", gameObject.name));
        piece.ChangeColorTemp(mouseOver);
    }

    private void OnMouseExit()
    {
        //Debug.Log(string.Format("{0} ������ ���콺 ����", gameObject.name));
        piece.ChangeColorTempBack();
    }


    
    private void OnMouseUpAsButton()
    {
        //if (piece.Belong.Turn != GameManager.Instance.turnState) return;
        if (GameManager.Instance.CurPlayer != piece.Belong) 
        { 
            Debug.Log("�⹰ ���� ���� �ƴ�. ��: " + GameManager.Instance.CurPlayer + "/ �⹰ ����: " + piece.Belong);  
            return; 
        }
        
        // ����: AI�� �Ͽ� �÷��̾ AI���� ���� �⹰�� ���� �� �ִ�.


        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
        {
            //Debug.Log(string.Format("{0} Ŭ��", gameObject.name));
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
                    //BeAttackedBy(PlaceManager.Instance.SelectedPiece);
                    // ���õ� �⹰�� ������
                    // (������ �Լ� ���ο��� ���� ���� ����)
                    PlaceManager.Instance.MoveProcess(PlaceManager.Instance.SelectedPiece, piece.place);

                }
            }

        }
    }
}
