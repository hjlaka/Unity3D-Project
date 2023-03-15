using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChessEvent : GameEvent
{
    public enum EventType { ATTACK, THREAT, DEFENCE, RETURN, CHECK, CHECKMATE, GAME_END }

    private EventType type;
    public EventType Type { get { return type; } set { type = value; } }

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

    public ChessEvent(EventType type, Piece subject, Piece target)
    {
        this.type = type;
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

    public string GetTypeAsString()
    {
        switch(type)
        {
            case EventType.ATTACK:
                return "Attack";
            case EventType.THREAT:
                return "threat";
            case EventType.DEFENCE:
                return "defence";
            case EventType.RETURN:
                return "return";
            case EventType.CHECK:
                return "check";
            case EventType.CHECKMATE:
                return "checkmate";
            case EventType.GAME_END:
                return "Game end";

            default: 
                return "noneType";

        }
    }

    public float GetEventTypeScore()
    {
        switch(type)
        {
            case EventType.ATTACK:
                return 109f;

            case EventType.DEFENCE:
                return 1f;

            case EventType.THREAT:
                return 1f;

            case EventType.GAME_END:
                return 999f;

            default:
                return 0f;

        }
    }
}
