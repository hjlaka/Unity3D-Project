using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeUnit : Unit
{
    [Header("LifeUnit")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private bool isFree = false;
    public bool IsFree
    {
        get { return isFree; }
        set 
        { 
            isFree = value;
            isOnGame = !isFree;
        }
    }
    [SerializeField]
    private Vector3 target;

    public UnityEvent OnWalk;

    protected virtual void Update()
    {
        if(isFree)
        {
            MoveToTarget(target);
        }
    }
    protected virtual void Walk(Vector3 directionVec)
    {
        transform.Translate(moveSpeed * Time.deltaTime * directionVec, Space.World);

        OnWalk?.Invoke();
    }

    private void MoveToTarget(Vector3 targetLocation)
    {

        if ((targetLocation - transform.position).sqrMagnitude > 2f)
        {
            Vector3 directionVec = targetLocation - transform.position;
            directionVec.y = 0;
            directionVec.Normalize();


            if (directionVec.sqrMagnitude != 0)
            {
                transform.forward = Vector3.Lerp(transform.forward, directionVec, 0.5f);
            }

            Walk(directionVec);
        }
    }

    private void Jump()
    {
        transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
    }
}
