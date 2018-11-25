using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAction : MonoBehaviour
{
    [SerializeField]
    private float _speedMultiplier;

    [SerializeField]
    private float _speedDuration;

    private AudioSource _cookieSound;

    private void Awake()
    {
        _cookieSound = GameObject.Find("InGameSounds").transform.Find("Cookie").GetComponent<AudioSource>();
    }

    public void MultiplyCharacterSpeed(Character character)
    {
        _cookieSound.Play();
        character.MultiplySpeed(_speedMultiplier, _speedDuration);
        character.StartSpeedParticles(_speedDuration);
    }
}
