using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    public int maxControllers = 4;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("CheckControllers");
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    IEnumerator CheckControllers()
    {
        while (true)
        {
            for (int i = 0; i < maxControllers; i++)
            {

            }
            yield return new WaitForSeconds(5);
        }
    }
}
