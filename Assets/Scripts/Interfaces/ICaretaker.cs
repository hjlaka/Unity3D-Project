using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICareTaker
{
    public void Add(IMemento memento);

    public IMemento Get();
}
