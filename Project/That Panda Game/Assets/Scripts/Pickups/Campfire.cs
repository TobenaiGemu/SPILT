using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    private static CampfireAction _campfireAction;

    private void Start()
    {
        _campfireAction = GameObject.Find("Collectables").transform.Find("Campfire").GetComponent<CampfireAction>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //For every frame that the character is triggering the campfire, call the campfire action
        if (other.transform.tag == "Character")
        {
            _campfireAction.StartRoasting(other.GetComponent<Character>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Try to stop roasting when the character exits
        if (other.transform.tag == "Character")
        {
            _campfireAction.StopRoasting(other.GetComponent<Character>());
        }
    }
}
