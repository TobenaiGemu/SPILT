using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _coinObj;

    [SerializeField]
    private float _spawnRate;

    [SerializeField]
    private Rect _spawnArea;

    private float _spawnTimer;

    private ObjectPool _coinPool;
    private int _initPoolAmmount;

    private List<GameObject> _activeCoins;

    private GameObject _planet;

	void Start ()
    {
        _activeCoins = new List<GameObject>();
        _coinPool = new ObjectPool(_coinObj, _initPoolAmmount);
        _planet = GameObject.Find("Planet");
	}

    public void Init()
    {
        _spawnTimer = 0;
    }
	
	public void SpawnerUpdate()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer > _spawnRate)
        {
            _spawnTimer = 0;
            SpawnCoin();
        }
	}

    private void ReturnCoin(GameObject obj)
    {
        _coinPool.ReturnObject(obj);
    }

    private void SpawnCoin()
    {
        GameObject coin = _coinPool.GetObject();
        _activeCoins.Add(coin);
        float x = Random.Range(_spawnArea.x, _spawnArea.x + _spawnArea.width);
        float y = Random.Range(_spawnArea.y, _spawnArea.y + _spawnArea.height);

        coin.transform.position = new Vector3(x, y, -30);

        RaycastHit hit;
        Physics.Raycast(coin.transform.position, Vector3.forward, out hit, Mathf.Infinity, 1 << 9);
        float z = hit.point.z;
        coin.transform.position = new Vector3(x, y, z);
        coin.transform.LookAt(_planet.transform);
        coin.SetActive(true);
    }

    public void Cleanup()
    {
        foreach (GameObject obj in _activeCoins)
        {
            ReturnCoin(obj);
        }
    }


}
