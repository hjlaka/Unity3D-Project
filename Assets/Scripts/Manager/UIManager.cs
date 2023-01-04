using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject unitList;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(unitList.activeSelf)
                unitList.SetActive(false);
            else 
                unitList.SetActive(true);
        }
    }
}
