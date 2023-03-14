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
            { 
                Debug.Log("자유의 몸이 되었다! "); 
                anim?.SetTrigger("Walk"); 
            }
            else
            {
                Debug.Log("자유 중지");
                anim?.SetTrigger("Idle");
            }
        }
    }
    [SerializeField]
    protected Vector3 targetLocation;

    public UnityEvent OnWalk;

    private Animator anim;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        if (isFree)
        {
            MoveToTarget();
        }
    }
    protected virtual void Walk()
    {
        transform.Translate(moveSpeed * Time.deltaTime * transform.forward, Space.Self); // 상태 머신으로 옮겨두기?

        //OnWalk?.Invoke();
    }
    protected virtual void Jump(Vector3 directionVec) 
    {
        // 제자리에서 점프했다가 목표 지점으로 하강.
    }
    protected virtual void Dash()
    {
        // 대쉬 애니메이션
    }

    public void SetMoveTarget(Vector3 targetLocation)
    {
        this.targetLocation = targetLocation;
    }

    public void MoveToTarget()
    {
        if ((targetLocation - transform.position).sqrMagnitude < 1f)
            return;
        
        //Vector3 direction = targetLocation - transform.position;

        transform.forward = targetLocation;
        /*// 각도가 바뀌었는지 확인
        if (Vector3.Dot(direction, transform.forward) > 0.001f)
        {
            Debug.Log("각도 변경");
            transform.forward = direction.normalized;
            //transform.forward = Vector3.Lerp(transform.forward, direction, 0.5f);
        }*/

        Walk();    
    }

    private void Jump()
    {
        transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
    }
}
