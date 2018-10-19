using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private static CoinAction _coinAction;
    private static CoinSpawner _coinSpawner;

    private void Start()
    {
        _coinAction = GameObject.Find("Collectables").transform.Find("Coin").GetComponent<CoinAction>();
        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<CoinSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.transform.parent.tag == "Player")
        {
            _coinAction.AddCoinToCharacter(other.gameObject.GetComponent<Character>());
            _coinSpawner.ReturnCoin(gameObject);

        }
    }
}
