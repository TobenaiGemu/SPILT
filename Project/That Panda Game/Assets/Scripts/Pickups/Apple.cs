using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour {

    private static SpeedAction _appleAction;
    private static Spawner _appleSpawner;

    private void Start()
    {
        _appleAction = GameObject.Find("Collectables").transform.Find("Apple").GetComponent<SpeedAction>();
        _appleSpawner = GameObject.Find("AppleSpawner").GetComponent<Spawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Character")
        {
            _appleAction.MultiplyCharacterSpeed(other.gameObject.GetComponent<Character>());
            _appleSpawner.ReturnObject(gameObject);
        }
    }
}
