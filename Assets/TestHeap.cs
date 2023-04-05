using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHeap : Heap<int>
{
    private void Start()
    {
        Push(new Node<int>(5, 5));
        Push(new Node<int>(111, 111));
        Push(new Node<int>(2, 2));
        Push(new Node<int>(13, 13));
        Push(new Node<int>(12, 12));
        Push(new Node<int>(11, 11));
    }
}
