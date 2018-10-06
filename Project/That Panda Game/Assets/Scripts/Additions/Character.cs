using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private GameObject _characterObj;
    private GameObject _characterPool;
    private User _assignedUser;
    private bool _isAssigned;

    public Character(CharacterType type)
    {
        _characterPool = GameObject.Find("AvailableCharacters");

        GameObject characters = GameObject.Find("AvailableCharacters");

       switch (type)
        {
            case CharacterType.Panda:
                _characterObj = characters.transform.Find("Panda").gameObject;
                break;
            case CharacterType.Lizard:
                _characterObj = characters.transform.Find("Lizard").gameObject;
                break;
            case CharacterType.Elephant:
                _characterObj = characters.transform.Find("Elephant").gameObject;
                break;
            case CharacterType.Pig:
                _characterObj = characters.transform.Find("Pig").gameObject;
                break;
        }
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
