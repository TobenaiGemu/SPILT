using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [SerializeField]
    private float _rotationSpeed;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    	
	}

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.down * _rotationSpeed * Time.deltaTime);
    }
}
