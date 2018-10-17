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

    [SerializeField]
    private GameObject _apple;
    private GameObject _applePoolObj;
    private List<GameObject> _applePool = new List<GameObject>();
    private int _applePoolAmmount = 5;
    [SerializeField]
    private float _appleSpawnRate;
    private float _appleTime;
    
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

        //_applePoolObj = GameObject.Find("ApplePool");
        //for (int i = 0; i < _applePoolAmmount; i++)
        //{
        //    GameObject apple = Instantiate(_apple);
        //    apple.SetActive(false);
        //    apple.transform.SetParent(_applePoolObj.transform, false);
        //    _applePool.Add(apple);
        //}
        //_appleTime = Time.time + _appleSpawnRate;
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

        //if (Time.time > _appleTime)
        //{
        //    _appleTime += _appleSpawnRate;
        //    GameObject apple = GetCoin();
        //    apple.SetActive(true);
        //    apple.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -40);
        //    apple.AddComponent<GravitySim>();
        //    apple.transform.LookAt(_planet.transform);
        //}
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

    GameObject GetApple()
    {
        if (_applePool.Count == 0)
        {
            GameObject apple = Instantiate(_apple);
            apple.transform.SetParent(_applePoolObj.transform, false);
            _applePool.Add(apple);
        }
        GameObject obj = _applePool[0];
        _applePool.RemoveAt(0);
        return obj;
    }
}
