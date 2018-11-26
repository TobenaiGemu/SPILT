using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private int _shakeCount;
    private bool _shake;
    private Vector3 _initPos;
    private Vector3 _nextPos;
    private TimeLerper _lerper;

	void Start ()
    {
        _lerper = new TimeLerper();
	}
	
	void Update ()
    {
        if (!_shake)
            return;

        if (_shakeCount < 5)
        {
            //Choose a random point in a unit circle and lerp towards it
            if (Camera.main.transform.position != _nextPos)
            {
                Camera.main.transform.position = _lerper.Lerp(_initPos, _nextPos, 0.01f);
            }
            else
            {
                _initPos = Camera.main.transform.position;
                _nextPos = new Vector3(Random.insideUnitCircle.x * 1, Random.insideUnitCircle.y * 1, -54);
                _lerper.Reset();
                _shakeCount++;
            }
        }
        //Reset to default position when camera has shaken 5 tiems
        else
        {
            _shake = false;
            transform.position = new Vector3(0, 0, -54);
        }
	}

    public void Shake()
    {
        _shake = true;
        _shakeCount = 0;
        _nextPos = new Vector3(0, 0, -54);
        _initPos = _nextPos;
        _lerper.Reset();
    }
}
