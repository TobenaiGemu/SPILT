using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLerper
{
    private float _startTime;
    private bool _startedLerp;

    public void Reset()
    {
        _startedLerp = false;
    }

    private float GetPercent(float timeToFinish)
    {
        if (!_startedLerp)
        {
            _startedLerp = true;
            _startTime = Time.time;
        }

        float lerpTime = Time.time - _startTime;
        return lerpTime / timeToFinish;
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
