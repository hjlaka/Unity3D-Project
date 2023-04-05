using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOriginator<T> where T : IMemento
{
    T SaveMemento(T memento);

    void ApplyMemento();
}
