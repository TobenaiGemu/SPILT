using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireAction : MonoBehaviour
{
    [SerializeField]
    private float _timeToRoast;
    [SerializeField]
    private float _timeToUnroast;
    [SerializeField]
    private float _knockBackMultiplier;
    [SerializeField]
    private float _knockJumpMultiplier;
    [SerializeField]
    private int _coinDropModifier;

    public void StartRoasting(Character character)
    {
        character.RoastMarshmallow(_timeToRoast, _timeToUnroast, _knockBackMultiplier, _knockJumpMultiplier, _coinDropModifier);
    }

    public void StopRoasting(Character character)
    {
        character.StopRoastingMarshmallow();
    }
}
