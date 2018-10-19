using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieAction : MonoBehaviour
{
    [SerializeField]
    private float _newSpeed;

    public void SlowDownCharacter(Character character)
    {
        character.ChangeSpeed(_newSpeed);
    }
}
