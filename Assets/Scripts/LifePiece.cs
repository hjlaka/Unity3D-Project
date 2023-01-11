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

    protected override void Walk()
    {
        base.Walk();

        Debug.Log(animator);
        animator?.SetTrigger("Walk");
    }


}
