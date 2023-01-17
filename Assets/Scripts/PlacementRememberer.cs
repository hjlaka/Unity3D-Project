using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementRememberer : MonoBehaviour, ICareTaker
{
    private List<Placement> placementList;

    private void Awake()
    {
        placementList = new List<Placement>();
    }
    public void Add(IMemento memento)
    {
        if(memento is Placement)
        {
            placementList.Add(memento as Placement);
            Debug.Log("위치 개수: " + placementList.Count);
        }
            
    }

    public IMemento Get()
    {
        return placementList[placementList.Count - 1];
    }
}
