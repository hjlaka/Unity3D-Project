using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : SingleTon<PlayerDataManager>
{
    private List<Piece> playerPieces;

    public List<CharacterData> unitList;

    [SerializeField]
    private PlayerUnitUI ui;

    public List<Piece> PlayerPieces 
    { get 
        { 
            return playerPieces; 
        } 
    }

    private void Awake()
    {
        playerPieces = new List<Piece>();
        unitList = new List<CharacterData>();
    }

    public void AddPlayerPiece(Piece piece)
    {
        playerPieces.Add(piece);

        AddUnit(piece.character);
    }

    public void AddUnit(CharacterData character)
    {
        unitList.Add(character);
        ui.AddUI(character);
    }

    public void RemoveUnit(CharacterData character)
    {
        unitList.Remove(character);
    }
}
