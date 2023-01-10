using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chess/GameSetting")]

public class GameData : ScriptableObject
{

    public int dataID;
    public TeamData team;
    public List<OpponentPiece> opponents;



    [Serializable]
    public struct OpponentPiece
    {
        public Piece piecePrefab;

        public CharacterData character;

        public Vector2Int location;
    }

}


