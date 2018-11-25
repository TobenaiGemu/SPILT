using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAction : MonoBehaviour{

    [SerializeField]
    private int _coinAmmount;

    private AudioSource _gummyBearSound;

    private void Awake()
    {
        _gummyBearSound = GameObject.Find("InGameSounds").transform.Find("GummyBear").GetComponent<AudioSource>();
    }

    public void AddCoinToCharacter(Character character)
    {
        _gummyBearSound.Play();
        character.AddCoins(_coinAmmount);
    }
}
