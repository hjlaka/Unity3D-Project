using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LifePiece : LifeUnit
{
    private Animator animator;
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected override void Walk(Vector3 directionVec)
    {
        base.Walk(directionVec);

        animator?.SetTrigger("Walk");
    }


}
