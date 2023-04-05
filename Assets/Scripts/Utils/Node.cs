using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    public float value;
    public T obj;


    public Node(float value, T obj)
    {
        this.value = value;
        this.obj = obj;
    }

}
