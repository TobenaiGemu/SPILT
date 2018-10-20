using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    private static CampfireAction _campfireAction;
    private static Spawner _campfireSpawner;

    private void Start()
    {
        _campfireAction = GameObject.Find("Collectables").transform.Find("Campfire").GetComponent<CampfireAction>();
        _campfireSpawner = GameObject.Find("CookieSpawner").GetComponent<Spawner>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Character")
        {
            _campfireAction.StartRoasting(other.GetComponent<Character>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Character")
        {
            _campfireAction.StopRoasting(other.GetComponent<Character>());
        }
    }
}
