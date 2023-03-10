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

        //TODO: ���� ����
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE)
        {
            Debug.Log("�⹰ ���� �ܰ谡 �ƴ�");
            return;
        }

        GameManager.Instance.ChangeTurn(GameManager.TurnState.RETURN);

        // �⺻������ �ι� �����Ѵ�.

        for (int i = 0; i < 2; i++)
        {
            Placement placement = placementCareTaker.Get();
            if (placement == null) return;
            Debug.Log("���� ���: " + placement.Piece);

            // ������ �� ���� ����
            // TODO: ������ ������ ���ٸ� �Ŀ� ��͹����� ���� / Ȥ�� �迭 ó���� ����
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

            // ���� ���� �����Ӹ� ����
            // ������ �������� �޸��信 �������� �ʴ´�.
            PlaceManager.Instance.MovePiece(returnPiece, returnPosition);

            Piece capturedPiece = placement.CapturedPiece;

            if (capturedPiece != null)
            {
                Place capturedPlace = placement.NextPosition;
                // �⹰ ����
                Debug.Log(string.Format("�⹰: {0} ��ġ: {1}", capturedPiece, capturedPlace));
                PlaceManager.Instance.MovePiece(capturedPiece, capturedPlace);

                capturedPiece.IsFree = false;

            }
        }
        //GameManager.Instance.ChangeGameState(GameManager.GameState.TURN_FINISHED);
        // �̺�Ʈ Ȯ�� ���� ���� �Ŵ������� ó��
    }
}
