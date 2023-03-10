using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlacementCareTaker))]
public class PlacementRememberer : MonoBehaviour, IOriginator<Placement>
{
    private PlacementCareTaker placementCareTaker;

    private void Awake()
    {
        placementCareTaker = GetComponent<PlacementCareTaker>();
    }
    public Placement SaveMemento(Placement memento)
    {
        placementCareTaker.Add(memento);
        return memento;
    }

    public void ApplyMemento()
    {

        //TODO: 조건 수정
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE)
        {
            Debug.Log("기물 선택 단계가 아님");
            return;
        }

        GameManager.Instance.ChangeTurn(GameManager.TurnState.RETURN);

        // 기본적으로 두번 복기한다.

        for (int i = 0; i < 2; i++)
        {
            Placement placement = placementCareTaker.Get();
            if (placement == null) return;
            Debug.Log("복구 대상: " + placement.Piece);

            // 연달은 수 먼저 복구
            // TODO: 오류의 여지가 없다면 후에 재귀문으로 변경 / 혹은 배열 처리로 변경
            if (placement.Subsequent != null)
            {
                Placement subsequent = placement.Subsequent;
                Piece subsequentPiece = subsequent.Piece;
                Place subsequentPosition = subsequent.PrevPosition;

                PlaceManager.Instance.MovePiece(subsequentPiece, subsequentPosition);

                Piece subsequentCaptured = subsequent.CapturedPiece;
                if (subsequentCaptured != null)
                {
                    PlaceManager.Instance.MovePiece(subsequentCaptured, subsequent.NextPosition);
                    subsequentCaptured.IsFree = false;
                }
            }

            Piece returnPiece = placement.Piece;
            Place returnPosition = placement.PrevPosition;

            // 연출 없이 움직임만 복구
            // 복기한 움직임은 메멘토에 저장하지 않는다.
            PlaceManager.Instance.MovePiece(returnPiece, returnPosition);

            Piece capturedPiece = placement.CapturedPiece;

            if (capturedPiece != null)
            {
                Place capturedPlace = placement.NextPosition;
                // 기물 복구
                Debug.Log(string.Format("기물: {0} 위치: {1}", capturedPiece, capturedPlace));
                PlaceManager.Instance.MovePiece(capturedPiece, capturedPlace);

                capturedPiece.IsFree = false;

            }
        }
        //GameManager.Instance.ChangeGameState(GameManager.GameState.TURN_FINISHED);
        // 이벤트 확인 동작 게임 매니저에서 처리
    }
}
