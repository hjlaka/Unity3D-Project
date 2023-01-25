using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    protected List<Piece> pieceList;

    [SerializeField]
    protected Unit coreUnit;

    public Unit CoreUnit { get { return coreUnit; } set { coreUnit = value; } }


    public virtual void AddPiece(Piece piece)
    {

    }

    public virtual void DoTurn()
    {

    }

}
