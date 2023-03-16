using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(null == instance)
            {
                GameObject gameObject = GameObject.Find("Manager");
                if(null == gameObject)
                {
                    gameObject = new GameObject();
                    gameObject.name = "Manager";
                }
                
                if(gameObject.GetComponent<T>() == null)
                    gameObject.AddComponent<T>();

                instance = gameObject.GetComponent<T>();

                //DontDestroyOnLoad(gameObject);
            }

            return instance;
        }
    }
}
