using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _object;

    [SerializeField]
    private float _spawnRate;

    [SerializeField]
    private Rect _spawnArea;

    private GameObject _planet;

    protected ObjectPool _objectPool;
    private List<GameObject> _activeObjects;

    private float _spawnTimer;
	
    void Awake()
    {
        _planet = GameObject.Find("Planet");
        _activeObjects = new List<GameObject>();
        _objectPool = new ObjectPool(_object, 10);
    }

    public void Tick()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer > _spawnRate)
        {
            _spawnTimer = 0;
            SpawnObject(_spawnArea);
        }
    }

    private void SpawnObject(Rect spawnArea)
    {
        GameObject obj = _objectPool.GetObject();
        _activeObjects.Add(obj);
        float x = Random.Range(spawnArea.x, spawnArea.x + spawnArea.width);
        float y = Random.Range(spawnArea.y, spawnArea.y + spawnArea.height);

        obj.transform.position = new Vector3(x, y, -30);

        RaycastHit hit;
        Physics.Raycast(obj.transform.position, Vector3.forward, out hit, Mathf.Infinity, 1 << 9);
        float z = hit.point.z;

        obj.transform.position = new Vector3(x, y, z - 1);
        obj.transform.LookAt(_planet.transform);
        obj.transform.SetParent(_planet.transform, true);
        Coin coin = obj.GetComponent<Coin>();
        if (coin != null)
        {
            coin.GetComponent<GravitySim>().enabled = true;
            coin.CanPickup();
        }
        obj.SetActive(true);
    }

    public GameObject GetCoin()
    {
        return _objectPool.GetObject();
    }

    public void Cleanup()
    {
        foreach (GameObject obj in _activeObjects)
            _objectPool.ReturnObject(obj);
        _activeObjects.Clear();
    }

    public void ReturnObject(GameObject obj)
    {
        _objectPool.ReturnObject(obj);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
