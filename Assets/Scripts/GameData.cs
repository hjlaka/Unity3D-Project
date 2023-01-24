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

    public EventDialogue eventDialogue;

    public List<CallingPiece> opponents;
    public List<CallingPiece> players;



    [Serializable]
    public struct CallingPiece
    {
        public Piece piecePrefab;

        public CharacterData character;

        public Vector2Int location;

        public List<string> calledDialogue;
    }

}


