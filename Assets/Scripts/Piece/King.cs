using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class King : Piece
{
    public UnityEvent OnCastlingL;
    public UnityEvent OnCastlingR;

    public void CastlingL()
    {
        OnCastlingL?.Invoke();
    }

    public void CastlingR()
    {
        OnCastlingR?.Invoke();
    }
}
