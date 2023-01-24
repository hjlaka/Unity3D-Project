using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Castling : SpecialMove
{
    public enum Type 
    { 
        LEFT = -1,
        RIGHT = 1 
    };

    private Piece rook;
    private Place targetPlace;
    private Type type;

    [SerializeField]
    private bool validity;

    public Castling(Type type) 
    {
        this.type = type;
    }

    public void Init()
    {
        validity = InitialValidate();
    }
    private bool InitialValidate()
    {
        Vector2Int location = owner.place.boardIndex;

        for (int i = 3; i < 8; i++)
        {
            if (location.x + (i * (int)type) < 0) return false;
            if (location.x + (i * (int)type) > owner.place.board.Size.x - 1) return false;

            if (owner.place.board.places[location.x + (i * (int)type), location.y].Piece is Rook)
            {
                rook = owner.place.board.places[location.x + i, location.y].Piece;
                targetPlace = owner.place.board.places[location.x + (3 * (int)type), location.y];
                return true;
            }
        }
        return false;
    }
    public override bool Validate()
    {
        if(!validity) return false;

        Vector2Int location = owner.place.boardIndex;

        if (owner.MoveCount == 0 && rook.MoveCount == 0)
        {
            for(int i = 1; i < 3; i++)
            {
                Place place = owner.place.board.places[location.x + (i * (int)type), location.y];
                if (place.Piece != null) return false;
                if (owner.returnHeat.ReturnOpponentHeat(place) > 0) return false;
            }
            return true;
        }
        else
        {
            validity = false;
            return false;
        }
    }
    public override Placement DoAction()
    {
        //∑Ë¿« ¿Ãµø
        Placement subsequent = new Placement(rook, rook.place, targetPlace, null, null);

        return subsequent;
    }
}
