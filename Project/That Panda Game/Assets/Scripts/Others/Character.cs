using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character : MonoBehaviour
{
    private static int _maxCoins;

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

    public void Start()
    {

    }

    public Character Init(GameObject charObj)
    {
        _characterObj = charObj;
        return this;
    }

    public void AddCoins(int ammount)
    {
        _coins += ammount;
        if (_coins >= _maxCoins)
            WinGame();
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
