using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaMarshmallow : MonoBehaviour
{
    [SerializeField]
    private float _knockBackForce;
    [SerializeField]
    private float _knockJumpForce;
    [SerializeField]
    private int _coinsToDrop;

    private bool _isAngewy;
    private GameObject _planet;

    private bool _crashed;
    private TimeLerper _lerper;
    private MarshShadow _shadow;
    private Vector3 _initPos;
    private Vector3 _targetPos;

    private bool _followShadow;

    private void Awake()
    {
        _planet = GameObject.Find("Planet");
        gameObject.SetActive(false);
        _lerper = new TimeLerper();
        _shadow = GameObject.Find("Shadow").GetComponent<MarshShadow>();
        _crashed = true;
    }

    public void GetVewyAngewy(Character follow = null)
    {
        if (_isAngewy)
            return;
        _followShadow = false;
        if (follow != null)
            _followShadow = true;
        _lerper.Reset();
        _crashed = false;
        transform.SetParent(GameObject.Find("Events").transform, true);
        _isAngewy = true;
        float x = Random.Range(-15, 15);
        float y = Random.Range(-15, 15);
        transform.position = new Vector3(x, y, -30);
        transform.up = -(_planet.transform.position - transform.position).normalized;
        RaycastHit hit;
        Physics.Raycast(transform.position, (transform.position - _planet.transform.position).normalized, out hit, Mathf.Infinity, 1 << 9);
        _targetPos = hit.point;
        transform.position = transform.up * 500;
        _initPos = transform.position;
        gameObject.SetActive(true);
        _shadow.StartMarshShadow(follow);
    }

    public bool HasCrashed()
    {
        return _crashed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Planet")
        {
            transform.SetParent(_planet.transform, true);
            _isAngewy = false;
            _shadow.Cleanup();
            _crashed = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_crashed && other.gameObject.tag == "Character")
        {
            other.transform.GetComponent<Character>().ApplyKnockBack((other.transform.position - transform.position), _knockBackForce, _knockJumpForce);
            other.transform.GetComponent<Character>().DropCoins(_coinsToDrop);
            gameObject.SetActive(false);
        }
    }

    void Update ()
    {
        if (_crashed)
            return;

        if (_followShadow)
        {
            _targetPos = _shadow.transform.position;
        }
        transform.position = _lerper.Lerp(_initPos, _targetPos, 10);
	}

}
