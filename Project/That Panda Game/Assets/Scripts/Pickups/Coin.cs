using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private static CoinAction _coinAction;
    private static Spawner _coinSpawner;

    public bool _canPickup;

    private void Start()
    {
        _coinAction = GameObject.Find("Collectables").transform.Find("Coin").GetComponent<CoinAction>();
        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
    }

    private void OnEnable()
    {
        _canPickup = false;
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
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_canPickup && other.transform.tag == "Character")
        {
            Debug.Log("Pickup");
            _coinAction.AddCoinToCharacter(other.gameObject.GetComponent<Character>());
            _coinSpawner.ReturnObject(gameObject);
        }
    }
}
