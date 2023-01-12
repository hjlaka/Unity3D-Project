using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRecognizer : MonoBehaviour
{
    protected readonly Piece controlled;

    protected MoveRecognizer(Piece controlled)
    {
        this.controlled = controlled;
    }

    protected bool IsTopOutLocation(Vector2Int curLocation, int boardHeight)
    {
        if (curLocation.y > boardHeight - 1)
            return true;
        else
            return false;
    }

    protected bool IsBottomOutLocation(Vector2Int curLocation)
    {
        if (curLocation.y < 0)
            return true;
        else
            return false;
    }

    protected bool IsLeftOutLocation(Vector2Int curLocation)
    {
        if (curLocation.x < 0)
            return true;
        else
            return false;
    }

    protected bool IsRightOutLocation(Vector2Int curLocation, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1)
            return true;
        else
            return false;
    }

    public bool RecognizeObstaclePiece(Vector2Int curLocation)
    {
        Piece targetPiece = controlled.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = controlled.place.board.places[curLocation.x, curLocation.y];

        if (targetPiece != null)
        {
            return true;
        }
        else
        {
            Debug.Log("�տ� ��ֹ� ����");

            controlled.AddMovable(targetPlace);
            //���� �̵�

            return false;
        }
    }

    public bool RecognizePieceOnlyInfluence(Vector2Int curLocation)
    {
        Piece targetPiece = controlled.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = controlled.place.board.places[curLocation.x, curLocation.y];

        if (targetPiece != null)
        {
            RecognizeDefendOrAttack(curLocation, targetPiece, targetPlace);

            return true;
        }
        else
        {
            // �⹰�� ���� ��� �̵��� �� ����
            // ������� ����
            controlled.AddInfluence(targetPlace);
            return false;
        }
    }
    // } �� �������� ���� �߰� -----------------------------------------------------------

    private void RecognizeDefendOrAttack(Vector2Int curLocation, Piece targetPiece, Place targetPlace)
    {
        if (targetPiece.team.TeamId == controlled.team.TeamId)
        {
            controlled.AddDefence(targetPiece);
            targetPiece.BeDefended(controlled);

            // ����� �� �ִ� �ڸ��� �̵��� �� ������ ����� ���� �ڸ��̴�.
            controlled.AddInfluence(targetPlace);

        }
        else
        {
            controlled.AddThreat(targetPiece);
            targetPiece.BeThreatened(controlled);

            // ������ �� �ִ� �ڸ��� �̵��� �� �ִ� �ڸ� �̱⵵ �ϴ�.
            // ������ �� �ִ� �ڸ��� ����� �� �ڸ��̴�.
            controlled.AddMovable(targetPlace);
            controlled.AddInfluence(targetPlace);

        }
    }

    public bool RecognizePiece(Vector2Int curLocation)
    {
        Piece targetPiece = controlled.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = controlled.place.board.places[curLocation.x, curLocation.y];

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
        controlled.AddMovable(targetPlace);
        controlled.AddInfluence(targetPlace);
    }


}
