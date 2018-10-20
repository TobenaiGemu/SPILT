using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private static CoinAction _coinAction;
    private static Spawner _coinSpawner;

    private void Start()
    {
        _coinAction = GameObject.Find("Collectables").transform.Find("Coin").GetComponent<CoinAction>();
        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Character")
        {
            _coinAction.AddCoinToCharacter(other.gameObject.GetComponent<Character>());
            _coinSpawner.ReturnObject(gameObject);
        }
    }
}
