using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T>
{

    private List<Node<T>> heapList;

    public int Count { get { return heapList.Count; } }

    public Heap()
    {
        heapList = new List<Node<T>>();
    }

    private bool Empty()
    {
        return heapList.Count == 0;
    }

    public void Clear()
    {
        heapList.Clear();
    }

    private void Swap(Node<T> valueA, Node<T> valueB)
    {
        Node<T> temp = valueA;
        valueA = valueB;
        valueB = temp;
    }

    private void PushHeap(Node<T> data)
    {
        Debug.Log(string.Format("들어온 노드 {0} / 값: {1} / 내용물: {2}", data, data.value, data.obj));
        heapList.Add(data);

        int curIndex = Count - 1;
        int parentIndex = (curIndex - 1) / 2;

        while(curIndex > 0)
        {
            if (heapList[curIndex].value > heapList[parentIndex].value)
            {
                Swap(heapList[curIndex], heapList[parentIndex]);
                curIndex = parentIndex;
            }
            else
            {
                break;
            }
        }

    }

    private Node<T> PopHeap()
    {
        if (Count <= 0) return null;

        Node<T> poped = heapList[0];

        Swap(heapList[0], heapList[Count - 1]);
        heapList.RemoveAt(Count - 1);

        int curIndex = 0;
        int leftChildIndex = curIndex * 2 + 1;
        int rightChildIndex = curIndex * 2 + 2;
        int compareIndex;

        while(curIndex < Count - 1)
        {
            if(rightChildIndex < Count)
            {
                if (heapList[leftChildIndex].value > heapList[rightChildIndex].value)
                    compareIndex = leftChildIndex;
                else
                    compareIndex = rightChildIndex;

                if (heapList[curIndex].value > heapList[compareIndex].value)
                {
                    Swap(heapList[curIndex], heapList[compareIndex]);
                    curIndex = compareIndex;
                }
                else
                {
                    break;
                }
            }
            else if (leftChildIndex < Count)
            {
                compareIndex = leftChildIndex;

                if (heapList[curIndex].value > heapList[compareIndex].value)
                {
                    Swap(heapList[curIndex], heapList[compareIndex]);
                    curIndex = compareIndex;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        return poped;
    }

    public void Push(in Node<T> data)
    {
        PushHeap(data);
    }

    public Node<T> Pop()
    {
        return PopHeap();
    }

    public Node<T> Top()
    {
        if (Count <= 0) return null;

        return heapList[0];
    }

}
