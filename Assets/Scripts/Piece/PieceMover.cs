using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Recognizer))]
public class PieceMover : MonoBehaviour
{

    protected Recognizer recognizer;

    private void Awake()
    {
        recognizer = GetComponent<Recognizer>();
    }
}
