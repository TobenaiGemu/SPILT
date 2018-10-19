using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAction : MonoBehaviour{

    [SerializeField]
    private int _coinAmmount;

    public void AddCoinToCharacter(Character character)
    {
        character.AddCoins(_coinAmmount);
    }
}
