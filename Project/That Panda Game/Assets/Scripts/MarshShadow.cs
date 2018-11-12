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
    private float _followToDistance;
    private bool _paused;

    void Start ()
    {
        _planet = GameObject.Find("Planet");
        _marshmallow = GameObject.Find("Events").transform.Find("MamaMarshmallow").gameObject;
        GetComponent<Renderer>().sharedMaterial.color = new Color(0, 0, 0, alpha);
        _lerper = new TimeLerper();
        gameObject.SetActive(false);
	}

    //Setup the shadow at the position the mama marshmallow will hit
    public void StartMarshShadow()
    {
        _followCharacter = null;
        _lerper.Reset();
        gameObject.SetActive(true);
        transform.position = _marshmallow.transform.up * 25;
        transform.LookAt(_planet.transform);
        transform.Rotate(-90, 0, 0);
        _initScale = new Vector3(0.1f, 1, 0.1f);
        _targetScale = new Vector3(1, 1, 1);
    }

    public void PauseShadow()
    {
        _paused = true;
    }

    public void ResumeShadow()
    {
        _paused = false;
    }

    public void FollowCharacter(Character follow)
    {
        _followCharacter = follow;
    }

    public void StopFollowingCharacter()
    {
        _followCharacter = null;
    }

    public void Cleanup()
    {
        _lerper.Reset();
        gameObject.SetActive(false);
    }

	void Update ()
    {
        if (_paused)
            return;
        //Follow character if not null
        if (_followCharacter != null)
        {
            transform.position = (_followCharacter.transform.position - _planet.transform.position).normalized * 25;
            transform.LookAt(_planet.transform);
            transform.Rotate(-90, 0, 0);
        }
        //Increase scale over time
        transform.localScale = _lerper.Lerp(_initScale, _targetScale, 9.5f);
	}
}
