using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMove : MoveRecognizer, IPieceMovable
{

    private bool canDoubleMove;

    public PawnMove(Piece controlled, bool canDoubleMove = true) : base(controlled)
    {
        this.canDoubleMove = canDoubleMove;
    }

    public void RecognizeRange(Vector2Int location, StateLists recognized)
    {
        Vector2Int boardSize = controlled.place.board.Size;
        recognizedLists = recognized;

        MoveForward(location + new Vector2Int(0, controlled.forwardY), boardSize.y);

        AttackDiagonalLT(location + new Vector2Int(-1, controlled.forwardY), boardSize.y);
        AttackDiagonalRT(location + new Vector2Int(1, controlled.forwardY), boardSize.y, boardSize.x);
    }

    public void RecognizeSpecialMove(Place newPlace)
    {
        Place oldPlace = controlled.place;
        if (oldPlace?.board != newPlace.board)
            canDoubleMove = true;
        else
            canDoubleMove = false;
        /*            Debug.Log("특수 이동 체크: " + canDoubleMove);
                if (canDoubleMove && controlled.MoveCount > 0)
                    canDoubleMove = false;
                Debug.Log("특수 이동 체크2: " + canDoubleMove);
        */
    }
    // 중간에 기물이 폰이 되면 두번 움직임을 허용할 것인가?
    // 조건 변경? 정해진 행에서만 두번 움직일 수 있게 하기?

    private void MoveForward(Vector2Int curLocation, int boardHeight)
    {

        if (IsTopOutLocation(curLocation, boardHeight)) return;
        if (IsBottomOutLocation(curLocation)) return;

        if (RecognizeObstaclePiece(curLocation)) return;

        // 기물이 없고, 두번 움직이는 조건이 충족된다면 한번 더 확인
        MoveDoubleForward(curLocation + new Vector2Int(0, controlled.forwardY), boardHeight);
    }

    private void AttackDiagonalLT(Vector2Int curLocation, int boardHeight)
    {
        if (IsLeftOutLocation(curLocation) ||
            IsTopOutLocation(curLocation, boardHeight) ||
            IsBottomOutLocation(curLocation))
            return;

        RecognizePieceOnlyInfluence(curLocation);
    }

    private void AttackDiagonalRT(Vector2Int curLocation, int boardHeight, int boardWidth)
    {
        if (IsRightOutLocation(curLocation, boardWidth) ||
            IsTopOutLocation(curLocation, boardHeight) ||
            IsBottomOutLocation(curLocation))
            return;

        RecognizePieceOnlyInfluence(curLocation);
    }


    private void MoveDoubleForward(Vector2Int curLocation, int boardHeight)
    {
        // 앞으로 두 칸 이동할 수 있는가?
        if (canDoubleMove)
        {
            // 벽이라면
            if (IsTopOutLocation(curLocation, boardHeight)) return;
            if (IsBottomOutLocation(curLocation)) return;

            // 기물이 있다면 종료, 기물이 없다면 이동할 수 있는 범위로 등록
            if (RecognizeObstaclePiece(curLocation)) return;

        }
    }



}
