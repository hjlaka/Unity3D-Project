using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    public const string managerObjectName = "Manager";
    private static T instance;
    public static T Instance
    {
        get
        {
            if(null == instance)
            {
                GameObject gameObject = GameObject.Find(managerObjectName);
                if(null == gameObject)
                {
                    gameObject = new GameObject();
                    gameObject.name = managerObjectName;
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
