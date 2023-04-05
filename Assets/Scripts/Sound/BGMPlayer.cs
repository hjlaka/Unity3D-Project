using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BGMPlayer : MonoBehaviour
{
    public UnityEvent OnStart;


    private void Start()
    {
        OnStart?.Invoke();
    }
}
