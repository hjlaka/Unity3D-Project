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

    private Place capturedPlace;

    public Place CapturedPlace { get { return capturedPlace; } }



    private Place prevPlace;
    public Place PrevPosition { get { return prevPlace; } }
    private Place nextPlace;
    public Place NextPosition { get { return nextPlace; } }

    // �迭�� ���� ���� �ִ�.
    private Placement subsequent;
    public Placement Subsequent { get { return subsequent; } }

    public Placement(Piece piece, Place prevPlace, Place nextPlace, Piece capturedPiece, Placement subsequent)
    {
        this.piece = piece;
        this.prevPlace = prevPlace;
        this.nextPlace = nextPlace;
        this.capturedPiece = capturedPiece;
        this.subsequent = subsequent;
    }

    public void SetSubsequent(Placement subsequent)
    {
        this.subsequent = subsequent;
    }

    public IMemento GetState()
    {
        return this;
    }

}
