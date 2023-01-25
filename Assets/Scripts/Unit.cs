using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected bool isOnGame;

    public bool IsOnGame { get { return isOnGame; } set { isOnGame = value;} }
    public virtual string GetName()
    {
        return name;
    }
}
