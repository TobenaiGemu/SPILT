using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private static CoinAction _coinAction;
    private static Spawner _coinSpawner;
    private static GameObject _planet;

    public bool _canPickup;

    private void Start()
    {
        _coinAction = GameObject.Find("Collectables").transform.Find("Coin").GetComponent<CoinAction>();
        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
        _planet = GameObject.Find("Planet");
    }

    private void Update()
    {
        if (transform.position.z > 0)
            _coinSpawner.ReturnObject(gameObject);
    }

    public void CanPickup()
    {
        _canPickup = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Planet")
        {
            _canPickup = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.SetParent(_planet.transform, true);
            _coinSpawner.RotateGameObject(gameObject);
            if (transform.position.y > 0)
                transform.forward *= -1;
            transform.Rotate(new Vector3(0, Random.Range(-30, 30), 0));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_canPickup && other.transform.tag == "Character")
        {
            _coinAction.AddCoinToCharacter(other.gameObject.GetComponent<Character>());
            _coinSpawner.ReturnObject(gameObject);
        }
    }
}
