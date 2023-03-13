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
            IsOnGame = !isFree;

            if (isFree)
            { Debug.Log("자유의 몸이 되었다! "); }
        }
    }
    [SerializeField]
    protected Vector3 targetLocation;

    public UnityEvent OnWalk;

    protected virtual void Update()
    {
        if (isFree)
        {
            MoveToTarget(targetLocation);
        }
    }
    protected virtual void Walk(Vector3 directionVec)
    {
        transform.Translate(moveSpeed * Time.deltaTime * directionVec, Space.World);

        OnWalk?.Invoke();
    }
    protected virtual void Jump(Vector3 directionVec) 
    {
        // 제자리에서 점프했다가 목표 지점으로 하강.
    }
    protected virtual void Dash()
    {
        // 대쉬 애니메이션
    }

    public void MoveToTarget(Vector3 targetLocation)
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
