using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Subject : MonoBehaviour, ISubject
{
    private List<UnitObserver> observers = new List<UnitObserver>();
    public ISubject NotifyObserver()
    {
        for(int i = 0; i < observers.Count; i++)
        {
            observers[i].StateUpdate(this);
        }
        return this;
    }

    public void RegisterObserver(IObserver observer)
    {
        if (observers.Contains(observer as UnitObserver)) return;

        observers.Add(observer as UnitObserver);

        // 이벤트 전송

        // 아군 / 적 / 오브젝트
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Add(observer as UnitObserver);
    }
}
