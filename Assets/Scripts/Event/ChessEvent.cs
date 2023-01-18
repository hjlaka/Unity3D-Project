using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessEvent : GameEvent
{
    public enum EventType { RETURN, CHECK, CHECKMATE, GAME_END }

    private EventType eventType;

    private Piece subject;
    public Piece Subject { get { return subject; } }
    private Piece target;
    public Piece Target { get { return target; } }
    //private List<Piece> participants;

    public ChessEvent()
    {
        subject = null;
        target = null;
    }

    public ChessEvent(EventType eventType, Piece subject, Piece target)
    {
        this.eventType = eventType;
        this.subject = subject;
        this.target = target;
    }

    public void SetSubject(Piece piece)
    {
        subject = piece;
    }
    public void SetTarget(Piece piece)
    {
        target = piece;
    }
    public void SetType(EventType type)
    {
        eventType = type;
    }
}
