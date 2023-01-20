using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> : MonoBehaviour
{
    struct Node
    {
        float value;
        T obj;
    }

    private List<Node> heapList;


    public void Add(float value, T obj)
    {

    }
}
