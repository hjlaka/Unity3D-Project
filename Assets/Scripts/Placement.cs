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
    public Piece Piece { get { return piece; } }


    private Piece capturedPiece;
    public Piece CapturedPiece { get { return capturedPiece; } }



    private Place prevPlace;
    public Place PrevPosition { get { return prevPlace; } }
    private Place nextPlace;
    public Place NextPosition { get { return nextPlace; } }

    public Placement(Piece piece, Place prevPlace, Place nextPlace, Piece capturedPiece)
    {
        this.piece = piece;
        this.prevPlace = prevPlace;
        this.nextPlace = nextPlace;
        this.capturedPiece = capturedPiece;
    }

    public IMemento GetState()
    {
        return this;
    }

}
