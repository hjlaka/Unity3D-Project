using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private List<Piece> playerPieces;

    public List<Piece> PlayerPieces 
    { get 
        { 
            return playerPieces; 
        } 
    }

    private void Awake()
    {
        playerPieces = new List<Piece>();
    }
}
