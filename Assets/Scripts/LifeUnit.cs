using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUnit : MonoBehaviour
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
        set { isFree = value; }
    }
    [SerializeField]
    private Vector3 target;

    protected virtual void Update()
    {
        if(isFree)
        {
            MoveToTarget(target);
        }
    }
    protected virtual void Walk(Vector3 directionVec)
    {
        transform.Translate(moveSpeed * Time.deltaTime * directionVec);
    }

    private void MoveToTarget(Vector3 targetLocation)
    {

        if ((targetLocation - transform.position).sqrMagnitude > 1f)
        {
            Vector3 directionVec = targetLocation - transform.position;
            directionVec.y = 0;
            directionVec.Normalize();


            if (directionVec.sqrMagnitude != 0)
            {
                transform.forward = Vector3.Lerp(transform.forward, directionVec, 0.2f);
            }

            Walk(directionVec);
        }
    }

    private void Jump()
    {
        transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
    }
}
