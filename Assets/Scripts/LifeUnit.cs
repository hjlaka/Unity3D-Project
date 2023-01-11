using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUnit : MonoBehaviour
{
    [Header("LifeUnit")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private bool isFree = false;
    [SerializeField]
    private Vector3 target;

    protected virtual void Update()
    {
        if(isFree)
        {
            MoveToTarget(target);
        }
    }
    protected virtual void Walk()
    {
        transform.Translate(moveSpeed * Time.deltaTime * transform.forward);
    }

    private void MoveToTarget(Vector3 targetLocation)
    {
        /*Vector3 direction =  targetLocation - transform.position;
        direction.y = 0;*/

        targetLocation.y = 0;

        transform.LookAt(targetLocation);

        while ((targetLocation - transform.position).sqrMagnitude > 1f)
        {
            Walk();
        }
    }
}
