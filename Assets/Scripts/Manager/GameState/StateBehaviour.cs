using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBehaviour<T> : MonoBehaviour
{
    protected T machine;

    private void Awake()
    {
        // �ӽ� �߰�. ������ �Ҵ��ϴ� ����� ����.
        machine = GetComponent<T>();
    }

    public abstract void StateEnter();


    public abstract void StateExit();


    public abstract void StateUpdate();

}
