using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLerper
{
    private float _timer;
    private float _newUpdateCheck;

    public void Reset()
    {
        _timer = 0;
    }

    private float GetPercent(float timeToFinish)
    {
        if (_newUpdateCheck != Time.time)
            _timer += Time.deltaTime;
        _newUpdateCheck = Time.time;
        return _timer / timeToFinish;
    }

    public Vector3 Lerp(Vector3 init, Vector3 end, float timeToFinish)
    {
        float lerpPercent = GetPercent(timeToFinish);
        return Vector3.Lerp(init, end, lerpPercent);
    }

    public float Lerp(float init, float end, float timeToFinish)
    {
        float lerpPercent = GetPercent(timeToFinish);
        return Mathf.Lerp(init, end, lerpPercent);
    }
}
