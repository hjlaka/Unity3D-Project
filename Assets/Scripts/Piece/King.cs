using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class King : Piece
{
    public UnityEvent OnCastlingL;
    public UnityEvent OnCastlingR;


    protected override void Awake()
    {
        base.Awake();

        movePattern = new EightWayMove(this, 1);
        pieceScore = 99;
    }

    public override void SetFree(bool isFree)
    {
        base.SetFree(isFree);

        if(isFree)
        {
            // 게임 매니저에게 신호를 보낸다.
        }
    }

    public void CastlingL()
    {
        OnCastlingL?.Invoke();
    }

    public void CastlingR()
    {
        OnCastlingR?.Invoke();
    }
}
