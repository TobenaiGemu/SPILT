using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleAction : MonoBehaviour
{
    [SerializeField]
    private int _coinDrop;

    private AudioSource _appleSound;

    private void Awake()
    {
        _appleSound = GameObject.Find("InGameSounds").transform.Find("Apple").GetComponent<AudioSource>();
    }

    public void DropCoinFromCharacter(Character character)
    {
        //Play the apple sounds, drop coins from character and start the sick bubbles animation
        _appleSound.Play();
        character.DropCoins(_coinDrop);
        character.StartSickBubbles();
    }
}
