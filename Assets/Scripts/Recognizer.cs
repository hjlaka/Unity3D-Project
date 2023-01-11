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

    // ----------------------------------------------------------- 폰 움직임을 위해 추가 { 
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
            Debug.Log("앞에 장애물 없다");

            piece.AddMovable(targetPlace);
            //연출 이동

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
            // 기물이 없는 경우 이동할 수 없음
            // 영향권은 맞음
            piece.AddInfluence(targetPlace);
            return false;
        }
    }
    // } 폰 움직임을 위해 추가 -----------------------------------------------------------

    private void RecognizeDefendOrAttack(Vector2Int curLocation, Piece targetPiece, Place targetPlace)
    {
        if (targetPiece.team.TeamId == piece.team.TeamId)
        {
            piece.AddDefence(targetPiece);
            targetPiece.BeDefended(piece);

            // 방어할 수 있는 자리는 이동할 수 없지만 영향권 내의 자리이다.
            piece.AddInfluence(targetPlace);

        }
        else
        {
            piece.AddThreat(targetPiece);
            targetPiece.BeThreatened(piece);

            // 공격할 수 있는 자리는 이동할 수 있는 자리 이기도 하다.
            // 공격할 수 있는 자리는 영향권 내 자리이다.
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
