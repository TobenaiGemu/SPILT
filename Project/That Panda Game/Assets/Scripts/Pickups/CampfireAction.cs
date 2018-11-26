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
        //Keep calling this function to roast the characters marshmallows over time
        character.RoastMarshmallow(_timeToRoast, _timeToUnroast, _knockBackMultiplier, _knockJumpMultiplier, _coinDropModifier);
    }

    public void StopRoasting(Character character)
    {
        //Try to stop roasting when it exits (if its already fully roasted this wont do anything)
        character.StopRoastingMarshmallow();
    }
}
