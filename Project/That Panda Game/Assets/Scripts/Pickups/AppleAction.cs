using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleAction : MonoBehaviour
{
    [SerializeField]
    private int _coinDrop;

    public void DropCoinFromCharacter(Character character)
    {
        character.DropCoins(_coinDrop);
        character.StartSickBubbles();
    }
}
