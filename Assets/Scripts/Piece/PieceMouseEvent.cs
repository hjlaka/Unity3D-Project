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

        // 기물 선택 단계에서 기물 선택
        // 기물 선택 후
        //      1. 기물 취소
        //      2. 기물 변경
        //      3. 기물 공격
        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
        {
            if (GameManager.Instance.curPlayer != piece.Belong)
            {
                Debug.Log("기물 주인 턴이 아님. 턴: " + GameManager.Instance.curPlayer + "/ 기물 주인: " + piece.Belong);
                return;
            }
            if (piece.Belong is AI)
            {
                Debug.Log("ai의 기물은 선택할 수 없음");
                return;
            }
            PlaceManager.Instance.SelectPiece(piece);
        }

        else if (GameManager.Instance.state == GameManager.GameState.SELECTING_PLACE)
        {
            // 자신이라면
            if (PlaceManager.Instance.SelectedPiece == piece)
            {
                Debug.Log("자신 클릭");
                PlaceManager.Instance.CancleSelectPiece();
            }
                

            // 같은 팀 기물이라면
            else if (piece.team.TeamId == PlaceManager.Instance.SelectedPiece.team.TeamId)
            {
                Debug.Log("같은 팀 클릭");
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
                    // 선택된 기물을 움직임
                    // (움직임 함수 내부에서 공격 연산 수행)
                    ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.ATTACK, PlaceManager.Instance.SelectedPiece, piece));
                    ChessEventManager.Instance.GetEvent();
                    DialogueManager.Instance.CheckDialogueEvent();
                    PlaceManager.Instance.MoveProcess(PlaceManager.Instance.SelectedPiece, piece.place);

                }
            }

        }
    }
}
