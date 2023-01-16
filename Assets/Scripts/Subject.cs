using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Subject
{
    void registerObserver(Observer observer);
    void removeObserver(Observer observer);
    void notifyObserver();
}
