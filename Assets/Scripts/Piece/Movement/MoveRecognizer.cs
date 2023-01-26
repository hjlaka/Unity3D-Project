using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRecognizer //�θ� �������̽� ���̱�?
{
    protected readonly Piece controlled;

    protected StateLists recognizedLists;

    public class ReachForPiece
    {
        public enum MeetType { ATTACK, DEFENCE, INFLUENCE };

        public Place reachedPlace;
        public List<Place> wayToPiece;
        public MeetType meetType;

        public ReachForPiece()
        {
            reachedPlace = null;
            wayToPiece = new List<Place>();
            meetType = MeetType.INFLUENCE;
        }
    }


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

    public bool RecognizePiece(Vector2Int curLocation)
    {
        Piece targetPiece = controlled.place.board.places[curLocation.x, curLocation.y].Piece;
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
        recognizedLists.AddMovable(targetPlace);
        recognizedLists.AddInfluenceable(targetPlace);

        // �߿��� �ڸ��� ��� �̺�Ʈ �߻�?
    }

    // �� ������ {
    public bool RecognizeObstaclePiece(Vector2Int curLocation)
    {
        Piece targetPiece = controlled.place.board.places[curLocation.x, curLocation.y].Piece;
        Place targetPlace = controlled.place.board.places[curLocation.x, curLocation.y];

        if (targetPiece != null)
        {
            recognizedLists.AddSpecial(targetPlace);
            return true;
        }
        else
        {
            Debug.Log("�տ� ��ֹ� ����");

            recognizedLists.AddMovable(targetPlace);
            recognizedLists.AddSpecial(targetPlace);
            //���� �̵�

            return false;
        }
    }

    public bool RecognizePieceOnlyInfluence(Vector2Int curLocation)
    {
        Piece targetPiece = controlled.place.board.places[curLocation.x, curLocation.y].Piece;
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
            recognizedLists.AddInfluenceable(targetPlace);
            return false;
        }
    }
    // } �� ������

    private void RecognizeDefendOrAttack(Vector2Int curLocation, Piece targetPiece, Place targetPlace)
    {
        if (targetPiece.team.TeamId == controlled.team.TeamId)
        {
            recognizedLists.AddDefending(targetPiece);

            // ����� �� �ִ� �ڸ��� �̵��� �� ������ ����� ���� �ڸ��̴�.
            recognizedLists.AddInfluenceable(targetPlace);

            IChessEventable chessEventable = recognizedLists as IChessEventable;
            chessEventable?.Defend(controlled, targetPiece);

        }
        else
        {
            recognizedLists.AddThreating(targetPiece);

            // ������ �� �ִ� �ڸ��� �̵��� �� �ִ� �ڸ� �̱⵵ �ϴ�.
            // ������ �� �ִ� �ڸ��� ����� �� �ڸ��̴�.
            recognizedLists.AddMovable(targetPlace);
            recognizedLists.AddInfluenceable(targetPlace);

            if (targetPiece.tag == "King")
            {
                // �տ� ���� ����
                
                IChessEventable chessEventable = recognizedLists as IChessEventable;
                chessEventable?.Check(targetPiece);
                
            }
            else
            {
                IChessEventable chessEventable = recognizedLists as IChessEventable;
                chessEventable?.Threat(controlled, targetPiece);
            }

        }
    }

    protected void AddMovable(Place place)
    {
        recognizedLists.AddMovable(place);
    }
}
