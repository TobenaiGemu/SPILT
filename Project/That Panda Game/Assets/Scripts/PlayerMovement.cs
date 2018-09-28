using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Vector3 _forwardVelocity;
    private Vector3 _strafeVelocity;

    public float speed = 5;
    public GameObject planet;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 prevPos = transform.position;

        _forwardVelocity = transform.forward * Input.GetAxis("Vertical");
        _strafeVelocity = transform.right * Input.GetAxis("Horizontal");

        transform.position += (_forwardVelocity + _strafeVelocity) * speed * Time.deltaTime;

        transform.up = transform.position - planet.transform.position;
    }
}
