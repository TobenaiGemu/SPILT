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

    protected ObjectPool _objectPool;
    private List<GameObject> _activeObjects;
    private GameObject _planet;

    private float _spawnTimer;
	
    void Awake()
    {
        _planet = GameObject.Find("Planet");
        _activeObjects = new List<GameObject>();
        _objectPool = new ObjectPool(_object, 10);
    }

    protected void MainUpdate()
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
        GameObject coin = _objectPool.GetObject();
        _activeObjects.Add(coin);
        float x = Random.Range(spawnArea.x, spawnArea.x + spawnArea.width);
        float y = Random.Range(spawnArea.y, spawnArea.y + spawnArea.height);

        coin.transform.position = new Vector3(x, y, -30);

        RaycastHit hit;
        Physics.Raycast(coin.transform.position, Vector3.forward, out hit, Mathf.Infinity, 1 << 9);
        float z = hit.point.z;
        coin.transform.position = new Vector3(x, y, z - 1);
        coin.transform.LookAt(_planet.transform);
        coin.SetActive(true);
    }

    public void Cleanup()
    {
        foreach (GameObject obj in _activeObjects)
            _objectPool.ReturnObject(obj);
        _activeObjects.Clear();
    }

    public void ReturnCoin(GameObject obj)
    {
        _objectPool.ReturnObject(obj);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
