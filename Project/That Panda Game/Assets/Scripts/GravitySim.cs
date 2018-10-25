using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySim : MonoBehaviour
{

    private GameObject[] _bodies;

    private Vector3 _velocity;
    private Vector3 _direction;

    private Rigidbody _rb;

    public static float gravConstant = 30;

	void Start ()
    {
        _bodies = GameObject.FindGameObjectsWithTag("GravityForce");
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
    }

    private void OnDisable()
    {
        _rb.velocity = Vector3.zero;
    }

    void FixedUpdate ()
    {
        foreach (GameObject obj in _bodies)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (obj.name != transform.gameObject.name)
            {
                _direction = rb.position - _rb.position;
                float distance = _direction.magnitude;
                float force = _rb.mass * rb.mass / Mathf.Pow(distance, 2) * gravConstant * Time.deltaTime;
                _rb.AddForce(_direction.normalized * force, ForceMode.Impulse);
            }
        }
    }
}
