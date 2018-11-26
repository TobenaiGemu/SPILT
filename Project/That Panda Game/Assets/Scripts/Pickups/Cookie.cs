using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    [SerializeField]
    private float zToDespawn;
    private static SpeedAction _cookieAction;
    private static Spawner _cookieSpawner;

    private void Start()
    {
        _cookieAction = GameObject.Find("Collectables").transform.Find("Cookie").GetComponent<SpeedAction>();
        _cookieSpawner = GameObject.Find("CookieSpawner").GetComponent<Spawner>();
    }

    private void Update()
    {
        //Return this object to the object pool if it goes beyond the border
        if (transform.position.z > zToDespawn * -1)
            _cookieSpawner.ReturnObject(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Do the cookie action if a character triggers this
        if (other.transform.tag == "Character")
        {
            _cookieAction.MultiplyCharacterSpeed(other.gameObject.GetComponent<Character>());
            _cookieSpawner.ReturnObject(gameObject);
        }
    }
}
