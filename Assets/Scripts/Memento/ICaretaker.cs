using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICareTaker<T> where T : IMemento
{
    public void Add(T memento);

    public T Get();
}
