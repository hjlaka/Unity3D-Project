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
        Debug.Log("마우스 뗌");
        piece.ChangeColorTempBack();
    }

    private void OnMouseUpAsButton()
    {

        // 기물 선택 단계에서 기물 선택
        // 기물 선택 후
        //      1. 기물 취소
        //      2. 기물 변경
        //      3. 기물 공격

        // 조건 체크
        if (GameManager.Instance.CurStateType != GameManager.GameState.ON_TURN)
        {
            Debug.Log("턴 단계가 아님");
            return;
        }

        if (GameManager.Instance.curPlayer is AI)
        {
            Debug.Log("AI의 차례");
            return;
        }  
        
        if (PlaceManager.Instance.SelectedPiece == null)    // 선택된 기물이 있는지 확인? 혹은 상태 확인?
        {
            Debug.Log("기물 선택함");
            SelectPiece(piece);
        }
        else
        {
            //선택된 기물이 있는 상태에서 다시 클릭
            // 자신이라면
            if (PlaceManager.Instance.SelectedPiece == piece)
            {
                CancelSelect();
            }

            // 같은 팀 기물이라면
            else if (piece.IsSameTeam(PlaceManager.Instance.SelectedPiece)) 
            {
                Debug.Log("같은 팀 클릭");
                CancelSelect();
                SelectPiece(piece);
            }
            // 다른 팀 기물이라면
            else
            {
                // 공격할 수 있다면
                if (piece.place.IsAttackableByCurPiece)
                {
                    //공격 
                    // 매니저에게 공격을 할 예정임을 알려줌.

                    Attack();
                }
            }
        }
    }

    private void SelectPiece(Piece piece)
    {
        if (piece.Belong != GameManager.Instance.curPlayer)
        {
            Debug.Log("기물 주인 턴이 아님. 턴: " + GameManager.Instance.curPlayer + "/ 기물 주인: " + piece.Belong);
            return;
        }
        PlaceManager.Instance.SelectPiece(piece);
    }

    private void CancelSelect()
    {
        Debug.Log("자신 클릭");
        PlaceManager.Instance.CancleSelectPiece();
    }

    private void Attack()
    {
        Debug.Log("공격");
        /*ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.ATTACK, PlaceManager.Instance.SelectedPiece, piece));
        ChessEventManager.Instance.GetEvent();
        DialogueManager.Instance.CheckDialogueEvent();
        PlaceManager.Instance.MoveProcess(PlaceManager.Instance.SelectedPiece, piece.place);*/
        PlaceManager.Instance.GetWill(PlaceManager.Instance.SelectedPiece, piece);
    }
}
