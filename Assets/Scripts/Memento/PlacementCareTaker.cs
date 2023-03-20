using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementCareTaker : MonoBehaviour, ICareTaker<Placement>
{
    [SerializeField]
    private Stack<Placement> mementoStack;
    //디버그용 리스트
    private List<Placement> mementoList;

    private void Awake()
    {
        mementoStack = new Stack<Placement>();
        // 디버그용
        mementoList = new List<Placement>();
    }
    public void Add(Placement memento)
    {
        mementoStack.Push(memento);

        // 디버그용
        mementoList.Add(memento);       
    }

    public Placement Get()
    {
        if(mementoStack.Count <= 0) 
            return default;

        Placement memento = mementoStack.Pop();

        // 디버그용
        mementoList.Remove(memento);

        return memento;
    }
}
