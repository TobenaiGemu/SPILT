using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private List<GameObject> _objectPool;
    private GameObject originalObj;

    public ObjectPool(GameObject obj, int initPoolAmmount)
    {
        for (int i = 0; i < initPoolAmmount; i++)
        {
            AddObject();
        }
    }

    public GameObject GetObject()
    {
        if (_objectPool.Count == 0)
            AddObject();
        GameObject obj = _objectPool[0];
        _objectPool.RemoveAt(0);
        return obj;
    }

    private GameObject AddObject()
    {
        GameObject clone = GameObject.Instantiate(originalObj);
        clone.SetActive(false);
        _objectPool.Add(clone);
        return clone;
    }

    public void ReturnObject(GameObject obj)
    {
        _objectPool.Add(obj);
    }
}
