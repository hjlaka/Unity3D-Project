using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject
{
    void registerObserver(IObserver observer);
    void removeObserver(IObserver observer);
    void notifyObserver();
}
