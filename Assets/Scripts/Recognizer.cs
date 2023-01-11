using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Piece))]
public class Recognizer : MonoBehaviour
{

    private Piece piece;

    private void Awake()
    {
        piece = GetComponent<Piece>();
    }

    public bool IsTopOutLocation(Vector2Int curLocation, int boardHeight)
    {
        if (curLocation.y > boardHeight - 1)
            return true;
        else
            return false;
    }

    public bool IsBottomOutLocation(Vector2Int curLocation)
    {
        if (curLocation.y < 0)
            return true;
        else
            return false;
    }

    public bool IsLeftOutLocation(Vector2Int curLocation)
    {
        if (curLocation.x < 0)
            return true;
        else
            return false;
    }

    public bool IsRightOutLocation(Vector2Int curLocation, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1)
            return true;
        else
            return false;
    }

    // ----------------------------------------------------------- �� �������� ���� �߰� { 
    public bool RecognizeObstaclePiece(Vector2Int curLocation)
    {
        Piece targetPiece = piece.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = piece.place.board.places[curLocation.x, curLocation.y];

        if (targetPiece != null)
        {
            return true;
        }
        else
        {
            Debug.Log("�տ� ��ֹ� ����");

            piece.AddMovable(targetPlace);
            //���� �̵�

            return false;
        }
    }

    public bool RecognizePieceOnlyInfluence(Vector2Int curLocation)
    {
        Piece targetPiece = piece.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = piece.place.board.places[curLocation.x, curLocation.y];

        if (targetPiece != null)
        {
            RecognizeDefendOrAttack(curLocation, targetPiece, targetPlace);

            return true;
        }
        else
        {
            // �⹰�� ���� ��� �̵��� �� ����
            // ������� ����
            piece.AddInfluence(targetPlace);
            return false;
        }
    }
    // } �� �������� ���� �߰� -----------------------------------------------------------

    private void RecognizeDefendOrAttack(Vector2Int curLocation, Piece targetPiece, Place targetPlace)
    {
        if (targetPiece.team.TeamId == piece.team.TeamId)
        {
            piece.AddDefence(targetPiece);
            targetPiece.BeDefended(piece);

            // ����� �� �ִ� �ڸ��� �̵��� �� ������ ����� ���� �ڸ��̴�.
            piece.AddInfluence(targetPlace);

        }
        else
        {
            piece.AddThreat(targetPiece);
            targetPiece.BeThreatened(piece);

            // ������ �� �ִ� �ڸ��� �̵��� �� �ִ� �ڸ� �̱⵵ �ϴ�.
            // ������ �� �ִ� �ڸ��� ����� �� �ڸ��̴�.
            piece.AddMovable(targetPlace);
            piece.AddInfluence(targetPlace);

        }
    }

    public bool RecognizePiece(Vector2Int curLocation)
    {
        Piece targetPiece = piece.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = piece.place.board.places[curLocation.x, curLocation.y];

        if (targetPiece != null)
        {
            RecognizeDefendOrAttack(curLocation, targetPiece, targetPlace);

            return true;
        }
        else
        {

            RecognizeMovableVoidPlace(curLocation, targetPlace);

            return false;
        }
    }

    private void RecognizeMovableVoidPlace(Vector2Int curLocation, Place targetPlace)
    {
        piece.AddMovable(targetPlace);
        piece.AddInfluence(targetPlace);
    }
}
