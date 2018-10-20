using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character : MonoBehaviour
{
    [SerializeField]
    private int _coinsToWin;

    public Vector3 Position
    {
        get
        {
            return _characterObj.transform.position;
        }
        private set { }
    }

    [SerializeField]
    private float _forwardSpeed;
    public float ForwardSpeed
    {
        get
        {
            return _forwardSpeed;
        }
        private set { }
    }

    [SerializeField]
    private float _backwardSpeedMultiplier;
    public float BackwardSpeedMultiplier
    {
        get
        {
            return _backwardSpeedMultiplier;
        }
        private set { }
    }

    [SerializeField]
    private float _knockBack;
    public float KnockBack
    {
        get
        {
            return _knockBack;
        }
        private set { }
    }

    [SerializeField]
    private float _knockJump;
    public float KnockJump
    {
        get
        {
            return _knockJump;
        }
        private set { }
    }

    private GameObject _characterObj;
    private GameObject _characterPool;
    private User _assignedUser;
    private bool _isAssigned;

    private int _coins;


    private float _speedMultiplier;
    public float SpeedMultiplier
    {
        get
        {
            return _speedMultiplier;
        }
        private set { }
    }

    private float _knockbackMultiplier;
    public float KnockbackMultiplier
    {
        get
        {
            return _knockbackMultiplier;
        }
        private set { }
    }

    private float _knockjumpMultiplier;
    public float KnockjumpMultiplier
    {
        get
        {
            return _knockjumpMultiplier;
        }
        private set { }
    }

    private float _speedMultiplierTimer;

    private float _timeToUnroast;
    private float _roastPercent;
    private TimeLerper _lerper;

    private bool _marshmallowRoasted;

    public void Start()
    {
        _characterPool = GameObject.Find("AvailableCharacters");
        _lerper = new TimeLerper();
    }

    public Character Init(GameObject charObj)
    {
        _characterObj = charObj;
        _speedMultiplier = 1;
        return this;
    }

    public void AddCoins(int ammount)
    {
        _coins += ammount;
        if (_coins >= _coinsToWin)
            WinGame();
    }

    public void MultiplySpeed(float speedMultiplier, float duration)
    {
        _speedMultiplier = speedMultiplier;
        _speedMultiplierTimer = duration;
        StartCoroutine(SpeedMultiplierCountdown());
    }

    public IEnumerator SpeedMultiplierCountdown()
    {
        while (_speedMultiplierTimer > 0)
        {
            _speedMultiplierTimer -= 1;
            yield return new WaitForSeconds(1);
        }
        _speedMultiplier = 1;
    }

    private void WinGame()
    {
        //This character won the game (ragdoll other characters, run dance animation, change scene to end game scene
    }

    public bool AttemptAssignToUser(User user)
    {
        if (!_isAssigned)
        {
            _assignedUser = user;
            _isAssigned = true;
            _characterObj.transform.SetParent(GameObject.Find("Players").transform.Find("Player" + _assignedUser.UserId), false);
            _characterObj.transform.localPosition = Vector3.zero;
            _characterObj.SetActive(true);
            return true;
        }
        return false;
    }

    public void RoastMarshmallow(float durationToRoast, float durationToUnroast, float knockbackMultiplier, float knockjumpMultiplier, float coinDrop)
    {
        if (_marshmallowRoasted)
            return;
        _roastPercent = _lerper.Lerp(0, 100, durationToRoast);
        Debug.Log("Roasting: " + _roastPercent);
        //TODO: Change opacity of roast texture over time
        if (_roastPercent >= 100)
        {
            _timeToUnroast = durationToUnroast;
            CompleteRoast(knockbackMultiplier, knockjumpMultiplier, coinDrop);
        }
    }

    public void StopRoastingMarshmallow()
    {
        if (_marshmallowRoasted)
            return;
        Debug.Log("MARSHMALLOWS UNROASTED!!!");
        _roastPercent = 0;
        _lerper.Reset();
        //TODO: Change texture back to normal;
    }

    private void CompleteRoast(float knockbackMultiplier, float knockjumpMultiplier, float coinDrop)
    {
        Debug.Log("MARSHMALLOWS ROASTED!!!");
        _marshmallowRoasted = true;
        _knockbackMultiplier = knockbackMultiplier;
        _knockjumpMultiplier = knockjumpMultiplier;
        StartCoroutine(UnroastMarshmallowCounter());
        //TODO: Change coin drop to that and add roasted timer
    }

    public IEnumerator UnroastMarshmallowCounter()
    {
        while (_timeToUnroast > 0)
        {
            _timeToUnroast -= 1;
            yield return new WaitForSeconds(1);
        }
        _marshmallowRoasted = false;
        StopRoastingMarshmallow();
    }

    public void Unassign()
    {
        if (_isAssigned)
        {
            _assignedUser = null;
            _isAssigned = false;
            _characterObj.transform.SetParent(_characterPool.transform, false);
            _characterObj.SetActive(false);
        }
    }

}
