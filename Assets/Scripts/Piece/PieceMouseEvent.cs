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
        //Debug.Log(string.Format("{0} 마우스 가리킴", gameObject.name));
        piece.ChangeColorTemp(mouseOver);
    }

    private void OnMouseExit()
    {
        //Debug.Log(string.Format("{0} 밖으로 마우스 나감", gameObject.name));
        piece.ChangeColorTempBack();
    }

    private void OnMouseUpAsButton()
    {
        /*if (!GameManager.Instance.isPlayerTurn)
        {
            Debug.Log("플레이어의 차례가 아닙니다.");
            return;
        }*/
        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
        {
            //Debug.Log(string.Format("{0} 클릭", gameObject.name));
            PlaceManager.Instance.SelectPiece(piece);
        }

        else if (GameManager.Instance.state == GameManager.GameState.SELECTING_PLACE)
        {
            // 자신이라면
            if (PlaceManager.Instance.SelectedPiece == this)
                PlaceManager.Instance.CancleSelectPiece();

            // 같은 팀 기물이라면
            else if (piece.team.TeamId == PlaceManager.Instance.SelectedPiece.team.TeamId)
            {
                PlaceManager.Instance.CancleSelectPiece();
                PlaceManager.Instance.SelectPiece(piece);
            }
            // 다른 팀 기물이라면
            else
            {
                // 공격할 수 있다면
                if (piece.place.IsAttackableByCurPiece)
                {
                    //공격 
                    //BeAttackedBy(PlaceManager.Instance.SelectedPiece);
                    PlaceManager.Instance.Attack(PlaceManager.Instance.SelectedPiece, piece);

                }
            }

        }
    }
}
