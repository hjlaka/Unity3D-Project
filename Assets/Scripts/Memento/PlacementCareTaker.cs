using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementCareTaker : MonoBehaviour, ICareTaker<Placement>
{
    [SerializeField]
    private Stack<Placement> mementoStack;
    //����׿� ����Ʈ
    private List<Placement> mementoList;

    private void Awake()
    {
        mementoStack = new Stack<Placement>();
        // ����׿�
        mementoList = new List<Placement>();
    }
    public void Add(Placement memento)
    {
        mementoStack.Push(memento);

        // ����׿�
        mementoList.Add(memento);       
    }

    public Placement Get()
    {
        if(mementoStack.Count <= 0) 
            return default;

        Placement memento = mementoStack.Pop();

        // ����׿�
        mementoList.Remove(memento);

        return memento;
    }
}
