using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusedSpiral : MonoBehaviour
{

	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(90,0,0));
	}
}
