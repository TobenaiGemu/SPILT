using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private List<GameObject> _objectPool;
    private GameObject _originalObj;

    public ObjectPool(GameObject obj, int initPoolAmmount)
    {
        _originalObj = obj;
        _objectPool = new List<GameObject>();
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
        GameObject clone = GameObject.Instantiate(_originalObj);
        clone.SetActive(false);
        _objectPool.Add(clone);
        return clone;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(null);
        _objectPool.Add(obj);
    }
}
