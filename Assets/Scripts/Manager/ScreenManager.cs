using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : SingleTon<ScreenManager>
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        //Screen.SetResolution(540, 960, true);
    }

}
