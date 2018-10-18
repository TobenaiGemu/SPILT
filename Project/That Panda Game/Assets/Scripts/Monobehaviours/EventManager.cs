using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    GameObject _planet;

    private bool _doEvents;

    [SerializeField]
    private GameObject _coin;
    private GameObject _coinPoolObj;
    private List<GameObject> _coinPool = new List<GameObject>();
    private int _coinPoolAmmount = 10;
    [SerializeField]
    private float _coinSpawnRate;
    private float _coinTime;
    
	// Use this for initialization
	void Start ()
    {
        _planet = GameObject.Find("Planet");

        _coinPoolObj = GameObject.Find("CoinPool");
        for (int i = 0; i < _coinPoolAmmount; i++)
        {
            GameObject coin = Instantiate(_coin);
            coin.SetActive(false);
            coin.transform.SetParent(_coinPoolObj.transform, false);
            coin.AddComponent<Coin>();
            _coinPool.Add(coin);
        }
        _coinTime = Time.time + _coinSpawnRate;
	}

    public void StartEvents()
    {
        _doEvents = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_doEvents)
            return;

        if (Time.time > _coinTime)
        {
            _coinTime += _coinSpawnRate;
            GameObject coin = GetCoin();
            coin.SetActive(true);
            coin.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -40);
            coin.AddComponent<GravitySim>();
            coin.transform.LookAt(_planet.transform);
        }
    }

    GameObject GetCoin()
    {
        if (_coinPool.Count == 0)
        {
            GameObject coin = Instantiate(_coin);
            coin.transform.SetParent(_coinPoolObj.transform, false);
            _coinPool.Add(coin);
        }
        GameObject obj = _coinPool[0];
        _coinPool.RemoveAt(0);
        return obj;
    }
}
