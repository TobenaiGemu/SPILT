using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaMarshmallow : MonoBehaviour
{
    private bool _isAngewy;
    private GameObject _planet;

    private bool _crashed;
    private TimeLerper _lerper;
    private MarshShadow _shadow;
    private Vector3 _initPos;
    private Vector3 _targetPos;

    private void Awake()
    {
        _planet = GameObject.Find("Planet");
        gameObject.SetActive(false);
        _lerper = new TimeLerper();
        _shadow = GameObject.Find("Shadow").GetComponent<MarshShadow>();
    }

    public void GetVewyAngewy()
    {
        if (_isAngewy)
            return;
        _isAngewy = true;
        float x = Random.Range(-15, 15);
        float y = Random.Range(-15, 15);
        transform.position = new Vector3(x, y, -30);
        transform.up = -(_planet.transform.position - transform.position).normalized;
        _targetPos = -transform.up * 25;
        transform.position = transform.up * 500;
        _initPos = transform.position;
        gameObject.SetActive(true);
        _shadow.StartMarshShadow();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Planet")
        {
            transform.SetParent(_planet.transform, true);
            _shadow.Cleanup();
            _crashed = true;
        }
    }

    void Update ()
    {
        if (_crashed)
            return;
        transform.position = _lerper.Lerp(_initPos, _targetPos, 10);
        Debug.Log(transform.position);
	}
}
