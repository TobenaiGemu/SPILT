using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshShadow : MonoBehaviour
{
    public float alpha;
    private GameObject _planet;
    private GameObject _marshmallow;

    private TimeLerper _lerper;

    private Vector3 _initScale;
    private Vector3 _targetScale;

    private Character _followCharacter;

    private void OnValidate()
    {

    }
    // Use this for initialization
    void Start ()
    {
        _planet = GameObject.Find("Planet");
        _marshmallow = GameObject.Find("Events").transform.Find("MamaMarshmallow").gameObject;
        GetComponent<Renderer>().sharedMaterial.color = new Color(0, 0, 0, alpha);
        _lerper = new TimeLerper();
        gameObject.SetActive(false);
	}
	
    public void StartMarshShadow(Character follow = null)
    {
        if (follow != null)
            _followCharacter = follow;
        _lerper.Reset();
        gameObject.SetActive(true);
        transform.position = _marshmallow.transform.up * 25;
        transform.LookAt(_planet.transform);
        transform.Rotate(-90, 0, 0);
        _initScale = new Vector3(0.1f,1,0.1f);
        _targetScale = new Vector3(0.4f, 1, 0.4f);
    }

    public void Cleanup()
    {
        _lerper.Reset();
        gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update ()
    {
        if (_followCharacter != null)
        {
            transform.position = (_followCharacter.transform.position - _planet.transform.position).normalized * 25;
        }

        transform.localScale = _lerper.Lerp(_initScale, _targetScale, 9.5f);
	}
}
