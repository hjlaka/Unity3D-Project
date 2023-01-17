using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementRememberer : MonoBehaviour, ICareTaker
{
    private Stack<Placement> placementStack;
    //디버그용 리스트
    private List<Placement> placementList;

    private void Awake()
    {
        placementStack = new Stack<Placement>();
        // 디버그용
        placementList = new List<Placement>();
    }
    public void Add(IMemento memento)
    {
        if(memento is Placement)
        {
            placementStack.Push(memento as Placement);
            Debug.Log("위치 개수: " + placementStack.Count);

            // 디버그용
            placementList.Add(memento as Placement);
            
        }
            
    }

    public IMemento Get()
    {
        if(placementStack.Count <= 0) 
            return null;

        Placement placement = placementStack.Pop();

        // 디버그용
        placementList.Remove(placement);

        return placement;
    }
}
