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
    [SerializeField]
    private float _positionStopsFollowing;
    [SerializeField]
    private Vector3 _rotation;

    private bool _isAngewy;
    private GameObject _planet;

    private Animator _animator;
    private Material _mat;

    private bool _crashed;
    private TimeLerper _lerper;
    private TimeLerper _alphaLerper;
    private MarshShadow _shadow;
    private Vector3 _initPos;
    private Vector3 _targetPos;
    private float _initDistance;
    private bool _paused;
    private GameScene _gameScene;

    private void Awake()
    {
        _planet = GameObject.Find("Planet");
        gameObject.SetActive(false);
        _lerper = new TimeLerper();
        _alphaLerper = new TimeLerper();
        _shadow = GameObject.Find("Shadow").GetComponent<MarshShadow>();
        _animator = GetComponent<Animator>();
        _mat = GetComponentInChildren<Renderer>().sharedMaterial;
        _gameScene = GameObject.Find("Scenes").transform.Find("GameScene").GetComponent<GameScene>();
        _crashed = true;
    }

    //Sets up the mama marshmallow to begin to move towards the planet
    public void GetVewyAngewy(Character follow = null)
    {
        if (_isAngewy)
            return;
        _animator.SetTrigger("Idle");
        _lerper.Reset();
        _alphaLerper.Reset();
        Color colour = _mat.color;
        colour.a = 1;
        _mat.color = colour;
        _crashed = false;
        
        _isAngewy = true;
        //sets the x,y position of the mama marshmallow to a random point on a plane
        float x = Random.Range(-15, 15);
        float y = Random.Range(-15, 15);
        transform.position = new Vector3(x, y, -30);
        transform.up = -(_planet.transform.position - transform.position).normalized;
        //gets the target point that the mama marshmallow will crash at on the planet
        RaycastHit hit;
        Physics.Raycast(transform.position, (transform.position - _planet.transform.position).normalized, out hit, Mathf.Infinity, 1 << 9);
        _targetPos = hit.point;
        transform.position = transform.up * 300;
        _initPos = transform.position;
        gameObject.SetActive(true);
        transform.Rotate(_rotation);
        //tells the shadow to start increasing in size at the point that marshmallow will hit, or at a characters position.
        _shadow.StartMarshShadow();
        if (follow != null)
            _shadow.FollowCharacter(follow);
        _initDistance = (transform.position - _shadow.transform.position).magnitude;
    }

    public bool HasCrashed()
    {
        return _crashed;
    }

    public void StopMarshmallow()
    {
        _isAngewy = false;
        _crashed = true;
        _shadow.Cleanup();
        gameObject.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        //knock back any character that collides with the marshmallow
        if (!_crashed && other.gameObject.tag == "Character")
        {
            //Get the direction to knock back the character towards
            Vector3 knockDirection = (other.transform.position - _targetPos);

            //If the character is directly under the marshmallow, the x and y will be 0
            if ((int)knockDirection.x == 0 && (int)knockDirection.y == 0)
                knockDirection = other.transform.forward;
            //Apply knockback and make character drop coins
            other.transform.GetComponent<Character>().ApplyKnockBack(knockDirection, _knockBackForce, _knockJumpForce);
            other.transform.GetComponent<Character>().DropCoins(_coinsToDrop);
        }

        //Begin the cleanup phase when the marshmallow hits the planet
        if (other.gameObject.name == "Planet")
        {
            _isAngewy = false;
            _shadow.Cleanup();
            _crashed = true;
            _animator.SetTrigger("Squeeesh");
            Camera.main.GetComponent<CameraShake>().Shake();
        }
    }

    public void Pause()
    {
        _paused = true;
        _shadow.PauseShadow();
    }

    public void Resume()
    {
        _paused = false;
        _shadow.ResumeShadow();
    }

    void Update()
    {
        if (_crashed || _paused)
        {
            if (_mat.color.a != 0)
            {
                Color colour = _mat.color;
                colour.a = _alphaLerper.Lerp(1,0,0.5f);
                _mat.color = colour;
                if (colour.a == 0)
                {
                    gameObject.SetActive(false);
                    transform.SetParent(GameObject.Find("Events").transform, true);
                    _gameScene.ResetMamaTimer();
                }
            }
            return;
        }

        //When the marshmallow gets to a certain distance from the planet, stop the shadow from following a character (if it is)
        if (transform.position.z > _positionStopsFollowing)
            _shadow.StopFollowingCharacter();

        //Make the marshmallow follow the shadow
        _targetPos = _shadow.transform.position;
        _initPos = _shadow.transform.up * _initDistance;
        transform.up = -(_planet.transform.position - transform.position).normalized;

        //Lerp the marshmallows position towards the desired point on the planet
        transform.position = _lerper.Lerp(_initPos, _targetPos, 5);
    }
}
