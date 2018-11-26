using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour {

    [SerializeField]
    private float zToDespawn;
    private static AppleAction _appleAction;
    private static Spawner _appleSpawner;

    private void Start()
    {
        _appleAction = GameObject.Find("Collectables").transform.Find("Apple").GetComponent<AppleAction>();
        _appleSpawner = GameObject.Find("AppleSpawner").GetComponent<Spawner>();
    }

    private void OnEnable()
    {
        //Since the same function spawns every type of item, some won't spawn in at the correct rotation (since the 'up' transform vector might not actually be up on the model), so it is done 'manually' here
        transform.Rotate(new Vector3(-90, 0, 0));
    }

    private void Update()
    {
        //Despawn apple at border
        if (transform.position.z > zToDespawn * -1)
            _appleSpawner.ReturnObject(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Do the apple action if a character triggers this
        if (other.transform.tag == "Character")
        {
            _appleAction.DropCoinFromCharacter(other.gameObject.GetComponent<Character>());
            _appleSpawner.ReturnObject(gameObject);
        }
    }
}
