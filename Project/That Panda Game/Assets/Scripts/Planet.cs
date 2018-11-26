using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [SerializeField]
    private float _rotationSpeed;
    private bool _paused;

    public void Pause()
    {
        _paused = true;
    }

    public void Resume()
    {
        _paused = false;
    }

    private void FixedUpdate()
    {
        //Rotate the planet over time :)
        if (_paused)
            return;
        transform.Rotate(Vector3.down * _rotationSpeed * Time.deltaTime);
    }
}
