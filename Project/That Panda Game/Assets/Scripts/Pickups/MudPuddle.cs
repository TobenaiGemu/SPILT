using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudPuddle : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            GameObject.Find("Collectables").transform.Find("MudPuddle").GetComponent<MudPuddleAction>().MultiplyCharacterSpeed(other.gameObject.GetComponent<Character>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {

        }
    }
}
