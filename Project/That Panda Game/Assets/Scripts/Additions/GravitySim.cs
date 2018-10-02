using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySim : MonoBehaviour
{

    private GameObject[] _bodies;

    private Vector3 _velocity;
    private Vector3 _direction;

    private Rigidbody _rb;

    public static float gravConstant = 1;

	void Start ()
    {
        _bodies = GameObject.FindGameObjectsWithTag("GravityForce");
        _rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
	    foreach (GameObject obj in _bodies)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (obj != this.transform.gameObject)
            {
                _direction = rb.position - _rb.position;
                float distance = _direction.magnitude;
                float force = _rb.mass * rb.mass / Mathf.Pow(distance, 2) * gravConstant * Time.deltaTime;
                _rb.AddForce(_direction.normalized * force, ForceMode.Impulse);
            }
        }
	}
}
