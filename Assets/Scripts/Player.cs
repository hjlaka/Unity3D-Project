using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    protected List<Piece> pieceList;


    public virtual void AddPiece(Piece piece)
    {

    }

    public virtual void DoTurn()
    {

    }
}
