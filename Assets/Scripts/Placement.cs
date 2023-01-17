using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Placement : IMemento
{
    // �̵��� �⹰
    // �̵��ϱ� �� ��, ��
    // �̵��� ��, ��
    private Piece piece;
    private Vector2Int prevPosition;
    private Vector2Int nextPosition;

    public Placement(Piece piece, Vector2Int prevPosition, Vector2Int nextPosition)
    {
        this.piece = piece;
        this.prevPosition = prevPosition;
        this.nextPosition = nextPosition;
    }

    public IMemento GetState()
    {
        return this;
    }

    public void SetState()
    {

    }
}
