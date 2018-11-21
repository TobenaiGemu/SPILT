﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHit : MonoBehaviour
{
    private bool _isActive;
    private TimeLerper _lerper;

    [SerializeField]
    private Vector3 _targetScale;
    [SerializeField]
    private float _speedToScale;
    [SerializeField]
    private float _delay;
    private float _delayTimer;

    private void Awake()
    {
        _lerper = new TimeLerper();
        gameObject.SetActive(false);
    }

    public void Hit()
    {
        _lerper.Reset();
        transform.localScale = Vector3.zero;
        transform.up = Camera.main.transform.position - transform.position;
        gameObject.SetActive(true);
        _isActive = true;
        _delayTimer = _delay;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!_isActive)
            return;
        _delayTimer -= Time.deltaTime;
        if (_delayTimer > 0)
            return;
        transform.localScale = _lerper.Lerp(Vector3.zero, _targetScale, _speedToScale);
        if (transform.localScale == _targetScale)
        {
            _isActive = false;
            gameObject.SetActive(false);
        }
	}
}
