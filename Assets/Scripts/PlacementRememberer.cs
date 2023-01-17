using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementRememberer : MonoBehaviour, ICareTaker
{
    private Stack<Placement> placementStack;
    //����׿� ����Ʈ
    private List<Placement> placementList;

    private void Awake()
    {
        placementStack = new Stack<Placement>();
        // ����׿�
        placementList = new List<Placement>();
    }
    public void Add(IMemento memento)
    {
        if(memento is Placement)
        {
            placementStack.Push(memento as Placement);
            Debug.Log("��ġ ����: " + placementStack.Count);

            // ����׿�
            placementList.Add(memento as Placement);
            
        }
            
    }

    public IMemento Get()
    {
        if(placementStack.Count <= 0) 
            return null;

        Placement placement = placementStack.Pop();

        // ����׿�
        placementList.Remove(placement);

        return placement;
    }
}
