﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSpin : MonoBehaviour
{
    [SerializeField]
    private float _speed;


	void Update ()
    {
        //Rotate the title over time ;)
        transform.Rotate(0, 0, _speed * Time.deltaTime);
	}
}
