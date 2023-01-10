using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chess/GameSetting")]

public class GameData : ScriptableObject
{

    public int dataID;
    public TeamData playerTeam;
    public TeamData opponentTeam;

    public List<OpponentPiece> opponents;
    public List<PlayerPiece> players;



    [Serializable]
    public struct OpponentPiece
    {
        public Piece piecePrefab;

        public CharacterData character;

        public Vector2Int location;
    }

    [Serializable]
    public struct PlayerPiece
    {
        public Piece piecePrefab;

        public CharacterData character;

        public Vector2Int location;
    }

}


