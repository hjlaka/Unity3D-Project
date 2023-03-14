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
                Debug.Log("������ ���� �Ǿ���! "); 
                anim?.SetTrigger("Walk"); 
            }
            else
            {
                Debug.Log("���� ����");
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
        transform.Translate(moveSpeed * Time.deltaTime * transform.forward, Space.Self); // ���� �ӽ����� �Űܵα�?

        //OnWalk?.Invoke();
    }
    protected virtual void Jump(Vector3 directionVec) 
    {
        // ���ڸ����� �����ߴٰ� ��ǥ �������� �ϰ�.
    }
    protected virtual void Dash()
    {
        // �뽬 �ִϸ��̼�
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
        /*// ������ �ٲ������ Ȯ��
        if (Vector3.Dot(direction, transform.forward) > 0.001f)
        {
            Debug.Log("���� ����");
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
