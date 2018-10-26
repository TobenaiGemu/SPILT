using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudPuddleAction : MonoBehaviour
{
    [SerializeField]
    private float _speedMultiplier;

    public void MultiplyCharacterSpeed(Character character)
    {
        character.MultiplySpeed(_speedMultiplier, 0.1f);
    }
}
