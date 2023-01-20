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
        //if (piece.Belong.Turn != GameManager.Instance.turnState) return;
        if (GameManager.Instance.CurPlayer != piece.Belong) 
        { 
            Debug.Log("기물 주인 턴이 아님. 턴: " + GameManager.Instance.CurPlayer + "/ 기물 주인: " + piece.Belong);  
            return; 
        }
        
        // 버그: AI의 턴에 플레이어가 AI에게 속한 기물을 누를 수 있다.


        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
        {
            //Debug.Log(string.Format("{0} 클릭", gameObject.name));
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
                    //BeAttackedBy(PlaceManager.Instance.SelectedPiece);
                    // 선택된 기물을 움직임
                    // (움직임 함수 내부에서 공격 연산 수행)
                    PlaceManager.Instance.MoveProcess(PlaceManager.Instance.SelectedPiece, piece.place);

                }
            }

        }
    }
}
