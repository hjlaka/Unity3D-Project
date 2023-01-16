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
        pieceScore = 3;
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
