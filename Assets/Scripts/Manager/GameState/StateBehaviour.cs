using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBehaviour<T> : MonoBehaviour
{
    protected T machine;

    private void Awake()
    {
        // 임시 추가. 일일이 할당하는 방법도 있음.
        machine = GetComponent<T>();
    }

    public abstract void StateEnter();


    public abstract void StateExit();


    public abstract void StateUpdate();

}
