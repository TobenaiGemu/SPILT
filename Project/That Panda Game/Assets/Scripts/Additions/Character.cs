using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private GameObject _characterObj;
    private User _assignedUser;
    private bool _isAssigned;

    public Character(CharacterType type)
    {
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

    public bool AssignToUser(User user)
    {
        if (!_isAssigned)
        {
            _assignedUser = user;
            _isAssigned = true;
            _characterObj.transform.parent = GameObject.Find("Players").transform.Find("Player" + _assignedUser.UserId);
            _characterObj.SetActive(true);
            _characterObj.transform.localPosition = new Vector3(0, 0, 0);
            return true;
        }
        return false;
    }

}
