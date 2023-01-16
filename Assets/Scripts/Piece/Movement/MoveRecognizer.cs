using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRecognizer //부모에 인터페이스 붙이기?
{
    protected readonly Piece controlled;

    protected StateLists recognizedLists;

    public class ReachForPiece
    {
        public enum MeetType { ATTACK, DEFENCE, INFLUENCE };

        public Place reachedPlace;
        public List<Place> wayToPiece;
        public MeetType meetType;

        /*public void ChangeMeetType(Place place)
        {
            if(place.piece == null)
            {
                // 연결된 함수가 없다면 필요한 함수만 부르기는 어렵다.
                // 최적화는 덜 되겠지만 함수 영향 범위 전체 재계산이 쉬운 선택이다.
            }
        }*/
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
            Debug.Log("앞에 장애물 없다");

            recognizedLists.AddMovable(targetPlace);
            //연출 이동

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
            // 기물이 없는 경우 이동할 수 없음
            // 영향권은 맞음
            recognizedLists.AddInfluenceable(targetPlace);
            return false;
        }
    }
    // } 폰 움직임을 위해 추가 -----------------------------------------------------------

    private void RecognizeDefendOrAttack(Vector2Int curLocation, Piece targetPiece, Place targetPlace)
    {
        if (targetPiece.team.TeamId == controlled.team.TeamId)
        {
            recognizedLists.AddDefending(targetPiece);
            //targetPiece.BeDefended(controlled);

            // 방어할 수 있는 자리는 이동할 수 없지만 영향권 내의 자리이다.
            recognizedLists.AddInfluenceable(targetPlace);

            IChessEventable chessEventable = recognizedLists as IChessEventable;
            chessEventable?.Defend();
        }
        else
        {
            recognizedLists.AddThreating(targetPiece);
            //targetPiece.BeThreatened(controlled);

            // 공격할 수 있는 자리는 이동할 수 있는 자리 이기도 하다.
            // 공격할 수 있는 자리는 영향권 내 자리이다.
            recognizedLists.AddMovable(targetPlace);
            recognizedLists.AddInfluenceable(targetPlace);

            if (targetPiece.PieceScore >= 99)
            {
                // 왕에 대한 공격
                
                IChessEventable chessEventable = recognizedLists as IChessEventable;
                chessEventable?.Check();
                
            }
            else
            {
                IChessEventable chessEventable = recognizedLists as IChessEventable;
                chessEventable?.Threat();
            }

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
        recognizedLists.AddMovable(targetPlace);
        recognizedLists.AddInfluenceable(targetPlace);

        // 중요한 자리일 경우 이벤트 발생?
    }

    protected void AddMovable(Place place)
    {
        recognizedLists.AddMovable(place);
    }


}
