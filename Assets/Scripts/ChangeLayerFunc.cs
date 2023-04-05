using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayerFunc : MonoBehaviour
{
    public void ChangeLayerRecursively(Transform trans, int layer)
    {
        trans.gameObject.layer = layer;
        foreach (Transform child in trans)
        {
            ChangeLayerRecursively(child, layer);
        }
    }
}
