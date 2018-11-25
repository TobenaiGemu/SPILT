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
        _appleSound.Play();
        character.DropCoins(_coinDrop);
        character.StartSickBubbles();
    }
}
