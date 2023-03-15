using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBehaviour<T> : MonoBehaviour
{
    protected T machine;

    protected virtual void Awake()
    {
        // 임시 추가. 일일이 할당하는 방법도 있음.
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
