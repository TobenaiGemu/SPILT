﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour {

    private static AppleAction _appleAction;
    private static Spawner _appleSpawner;

    private void Start()
    {
        _appleAction = GameObject.Find("Collectables").transform.Find("Apple").GetComponent<AppleAction>();
        _appleSpawner = GameObject.Find("AppleSpawner").GetComponent<Spawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Planet")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (other.transform.tag == "Character")
        {
            _appleAction.DropCoinFromCharacter(other.gameObject.GetComponent<Character>());
            _appleSpawner.ReturnObject(gameObject);
        }
    }
}
