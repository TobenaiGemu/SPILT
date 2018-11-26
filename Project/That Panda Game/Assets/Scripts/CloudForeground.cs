using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudForeground : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private Vector2 _offset;

    private Material _mat;

    private void Awake()
    {
        _mat = GetComponent<Renderer>().material;
        _offset = Vector3.zero;
    }

    private void Update ()
    {
        //Offset the cloud material over time to give the illusion of movement
        _offset.x += _speed * Time.deltaTime;
        _mat.SetTextureOffset("_MainTex", _offset);
	}
}
