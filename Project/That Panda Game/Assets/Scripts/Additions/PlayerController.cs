using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private GameObject _planet;

    private Joystick _joystick;

    //Velocity for forward/side movements
    private Vector3 _forwardVelocity;
    private Vector3 _strafeVelocity;

    private float _speed = 5;

    public void Init(float speed, GameObject planet)
    {
        _speed = speed;
        _planet = planet;
    }

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Set the velocity vectors to the correct direction and magnitude (based on input and speed)
        _forwardVelocity = transform.forward * Input.GetAxis("Vertical") * _speed;
        _strafeVelocity = transform.right * Input.GetAxis("Horizontal") * _speed;

        //add the velocities to the position
        transform.position += (_forwardVelocity + _strafeVelocity)  * Time.deltaTime;

        //Rotate towards the centre of the planet
        transform.up = transform.position - _planet.transform.position;
    }
}
