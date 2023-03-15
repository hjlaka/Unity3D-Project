using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBehaviour<T> : MonoBehaviour
{
    protected T machine;

    protected virtual void Awake()
    {
        // �ӽ� �߰�. ������ �Ҵ��ϴ� ����� ����.
        machine = GetComponentInParent<T>();
    }

    public abstract StateBehaviour<T> Handle();

    public abstract void StateEnter();


    public abstract void StateExit();


    public virtual void StateUpdate() 
    { 
        // Do Nothing
    }

}
