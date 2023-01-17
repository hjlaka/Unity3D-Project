using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOriginator
{
    IMemento SaveMemento(IMemento memento);
}
