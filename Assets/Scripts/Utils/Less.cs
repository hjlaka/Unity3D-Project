using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Less : Compare
{
    public override bool CompareBetween(float a, float b)
    {
        return a < b;
    }
}
