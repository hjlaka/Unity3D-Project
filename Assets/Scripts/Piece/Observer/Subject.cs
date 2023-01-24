using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Subject : MonoBehaviour, ISubject
{
    private List<UnitObserver> observers = new List<UnitObserver>();
    public ISubject notifyObserver()
    {
        for(int i = 0; i < observers.Count; i++)
        {
            observers[i].StateUpdate(this);
        }
        return this;
    }

    public void registerObserver(IObserver observer)
    {
        if (observers.Contains(observer as UnitObserver)) return;

        observers.Add(observer as UnitObserver);

        // �̺�Ʈ ����

        // �Ʊ� / �� / ������Ʈ
    }

    public void removeObserver(IObserver observer)
    {
        observers.Add(observer as UnitObserver);
    }
}
