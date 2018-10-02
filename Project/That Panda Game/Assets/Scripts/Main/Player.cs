using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GravitySim))]
public class Player : MonoBehaviour
{
    public enum CharacterType {Panda, Lizard, Elephant, Pig}

    private GameObject[] _players;

    //Character being controlled
    public CharacterType Character { get; private set; }
    private Joystick _joystick;

    //Movement/Controller input
    public float moveSpeed = 20;
    private PlayerController _controller;

    public GameObject planet;

	void Start ()
    {
        //Find all other players
        _players = GameObject.FindGameObjectsWithTag("Player");

        //Check for a planet object
        if (planet == null)
            planet = GameObject.Find("Planet");

        //Check for a player controller.
        _controller = GetComponent<PlayerController>();
		if (_controller == null)
            _controller = gameObject.AddComponent<PlayerController>();
        _controller.Init(moveSpeed, planet);

        //Check for a gravity sim
        if (GetComponent<GravitySim>() == null)
            gameObject.AddComponent<GravitySim>();


	}
	
	void Update ()
    {
		
	}
}
