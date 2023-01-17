using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Placement : IMemento
{
    // 이동한 기물
    // 이동하기 전 줄, 열
    // 이동한 줄, 열
    private Piece piece;
    public Piece Piece { get { return piece; } }
    private Place prevPlace;
    public Place PrevPosition { get { return prevPlace; } }
    private Place nextPlace;
    public Place NextPosition { get { return nextPlace; } }

    public Placement(Piece piece, Place prevPlace, Place nextPlace)
    {
        this.piece = piece;
        this.prevPlace = prevPlace;
        this.nextPlace = nextPlace;
    }

    public IMemento GetState()
    {
        return this;
    }

}
