using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private List<GameObject> _objectPool;
    private GameObject _originalObj;

    private GameObject _poolObj;

    public ObjectPool(GameObject obj, int initPoolAmmount)
    {
        _poolObj = GameObject.Find("ObjectPool");
        _originalObj = obj;
        _objectPool = new List<GameObject>();
        for (int i = 0; i < initPoolAmmount; i++)
        {
            AddObject();
        }
    }

    //Gets an object from the pool
    public GameObject GetObject()
    {
        if (_objectPool.Count == 0)
            AddObject();
        GameObject obj = _objectPool[0];
        _objectPool.RemoveAt(0);
        return obj;
    }

    //Creates an object in the pool
    private GameObject AddObject()
    {
        GameObject clone = GameObject.Instantiate(_originalObj);
        clone.SetActive(false);
        _objectPool.Add(clone);
        clone.transform.SetParent(_poolObj.transform, true);
        return clone;
    }

    //Returns an object to the pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(_poolObj.transform);
        _objectPool.Add(obj);
    }
}
