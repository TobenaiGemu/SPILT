using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudPuddle : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        //Trigger the mud puddle action while the character stays in the mud
        if (other.gameObject.tag == "Character")
        {
            GameObject.Find("Collectables").transform.Find("MudPuddle").GetComponent<MudPuddleAction>().MultiplyCharacterSpeed(other.gameObject.GetComponent<Character>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            //other.transform.Find("MudSlowPS").GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            //other.transform.Find("MudSlowPS").GetComponent<ParticleSystem>().Stop();
        }
    }
}
