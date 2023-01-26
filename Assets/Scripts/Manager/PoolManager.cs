using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingleTon<PoolManager>
{
    public struct PoolableInfo
    {
        public GameObject prefab;
        public int count;
    }

    [SerializeField]
    private List<PoolableInfo> poolableList = new List<PoolableInfo>();

    private Dictionary<string, Stack<GameObject>> poolDic;

    private void Awake()
    {
        poolDic = new Dictionary<string, Stack<GameObject>>();
    }
    private void Start()
    {
        
    }
    private void CreatePool()
    {
        for(int i = 0; i < poolableList.Count; i++)
        {
            Stack<GameObject> stack = new Stack<GameObject>();

            for(int j = 0; j < poolableList[i].count; j++)
            {
                GameObject instance = Instantiate(poolableList[i].prefab);
                instance.SetActive(false);
                stack.Push(instance);
            }
            poolDic.Add(poolableList[i].prefab.name, stack);
  
        }
    }
    public GameObject Get(GameObject prefab)
    {
        Stack<GameObject> stack = poolDic[prefab.name];
        if(stack?.Count > 0)
        {
            GameObject instance = stack.Pop();
            instance.gameObject.SetActive(true);
            return instance;
        }

        return null;
    }

    public void Return(GameObject instance)
    {
        Stack<GameObject> stack = poolDic[instance.name];
        instance.SetActive(false);
        stack.Push(instance);
    }

}
