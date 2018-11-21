using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAction : MonoBehaviour
{
    [SerializeField]
    private float _speedMultiplier;

    [SerializeField]
    private float _speedDuration;

    public void MultiplyCharacterSpeed(Character character)
    {
        character.MultiplySpeed(_speedMultiplier, _speedDuration);
        character.StartWhooshing();
    }
}
