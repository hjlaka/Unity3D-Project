using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> : MonoBehaviour
{

    private List<Node<T>> heapList;

    [SerializeField]
    private Compare comp;

    public int Count { get { return heapList.Count; } }

    public Heap()
    {
        heapList = new List<Node<T>>();

        this.comp = new Greater();

    }
    public Heap(Compare comp)
    {
        heapList = new List<Node<T>>();

        this.comp = comp;

    }


    private bool Empty()
    {
        return heapList.Count == 0;
    }

    public void Clear()
    {
        heapList.Clear();
    }

    private void Swap( Node<T> valueA,  Node<T> valueB) 
    {
        Debug.Log("A, B " + valueA.value + " " + valueB.value);
        Node<T> temp = valueA;
        valueA = valueB;
        valueB = temp;
        Debug.Log("A, B " + valueA.value + " " + valueB.value);
    }

    private void SwapInList(int indexA, int indexB)
    {
        Node<T> temp = heapList[indexA];
        heapList[indexA] = heapList[indexB];
        heapList[indexB] = temp;
    }

    private void PushHeap(Node<T> data)
    {
        heapList.Add(data);

        int curIndex = Count - 1;


        while(curIndex > 0)
        {
            int parentIndex = (curIndex - 1) / 2;

            if (comp.CompareBetween(heapList[curIndex].value, heapList[parentIndex].value))
            {
                SwapInList(curIndex, parentIndex);
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

        SwapInList(0, Count - 1);
        heapList.RemoveAt(Count - 1);

        int curIndex = 0;
        int leftChildIndex = curIndex * 2 + 1;
        int rightChildIndex = curIndex * 2 + 2;
        int compareIndex;

        while(curIndex < Count - 1)
        {
            if(rightChildIndex < Count)
            {
                if (comp.CompareBetween(heapList[leftChildIndex].value, heapList[rightChildIndex].value))
                    compareIndex = leftChildIndex;
                else
                    compareIndex = rightChildIndex;

                if (comp.CompareBetween(heapList[curIndex].value, heapList[compareIndex].value))
                {
                    SwapInList(curIndex, compareIndex);
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

                if (comp.CompareBetween(heapList[curIndex].value, heapList[compareIndex].value))
                {
                    SwapInList(curIndex, compareIndex);
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

    public Node<T> GetNode(int index)
    {
        if (index > Count - 1) return null;

        return heapList[index];

    }

    public void PrintHeap()
    {
        int floorNum = 0;
        string debug = "내용물 순서";

        for(int i = 0; i < Count; i++)
        {
            if (i == floorNum)
            {
                Debug.Log(debug);
                debug = "";
                floorNum = floorNum * 2 + 1;
            }
            debug += "(" + heapList[i].value + "/" + heapList[i].obj + ") ";

        }

        Debug.Log(debug);
    }

}
