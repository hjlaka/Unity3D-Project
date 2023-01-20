using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected GameManager.TurnState teamTurn;

    [SerializeField]
    protected List<Piece> pieceList;

    public void SetTeamTurn(GameManager.TurnState turnDirection)
    {
        teamTurn = turnDirection;
    }

    public virtual void AddPiece(Piece piece)
    {

    }

    public virtual void DoTurn()
    {

    }
}
