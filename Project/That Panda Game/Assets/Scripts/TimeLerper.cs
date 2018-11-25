using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLerper
{
    private float _timer;

    public void Reset()
    {
        _timer = 0;
    }

    private float GetPercent(float timeToFinish)
    {
        _timer += Time.deltaTime;
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
