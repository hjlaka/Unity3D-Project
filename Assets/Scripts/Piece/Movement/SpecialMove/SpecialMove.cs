using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialMove : MonoBehaviour
{
    protected Piece owner;
    public abstract bool Validate();
    public abstract Placement DoAction();
}
