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
        if (transform.position.z > zToDespawn * -1)
            _cookieSpawner.ReturnObject(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Character")
        {
            _cookieAction.MultiplyCharacterSpeed(other.gameObject.GetComponent<Character>());
            _cookieSpawner.ReturnObject(gameObject);
        }
    }
}
