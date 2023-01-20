using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected GameManager.TurnState turn;
    public GameManager.TurnState Turn { get { return turn; } }

    [SerializeField]
    protected List<Piece> pieceList;

    public void SetTeamTurn(GameManager.TurnState turnDirection)
    {
        turn = turnDirection;
    }

    public virtual void AddPiece(Piece piece)
    {

    }

    public virtual void DoTurn()
    {

    }
}
