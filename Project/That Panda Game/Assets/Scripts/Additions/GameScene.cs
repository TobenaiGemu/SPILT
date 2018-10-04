using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterType {Panda, Lizard, Elephant, Pig}

public class GameScene : Scene
{
    private Dictionary<CharacterType, Character> _characters = new Dictionary<CharacterType, Character>();

    private SceneManager _sceneManager;

    public GameObject Planet { get; private set; }

    private Canvas _canvas;

    public GameScene(SceneManager sceneManager)
        :base(sceneManager)
    {
        _characters.Add(CharacterType.Panda, new Character(CharacterType.Panda));
        _characters.Add(CharacterType.Lizard, new Character(CharacterType.Lizard));
        _characters.Add(CharacterType.Elephant, new Character(CharacterType.Elephant));
        _characters.Add(CharacterType.Pig, new Character(CharacterType.Pig));

        _sceneManager = sceneManager;
    }

    public override void Initialize()
    {
        Planet = GameObject.Find("Planet");
        _canvas = GameObject.Find("PressToJoin").GetComponent<Canvas>();
        foreach (User user in SceneManager.Users)
            user.ChangeState("PlayerState");
    }

    public override void Update()
    {
        foreach (User user in SceneManager.Users)
        {
            if (user != null)
                user.Update();
        }
    }

    public bool AttemptCharacterAssign(CharacterType type, User user)
    {
        bool isOK = _characters[type].AssignToUser(user);

        if (isOK)
        {
            switch (type)
            {
                case CharacterType.Panda:
                    _canvas.transform.Find("Panda").GetComponent<Text>().text = "Panda: Joined";
                    break;
                case CharacterType.Lizard:
                    _canvas.transform.Find("Lizard").GetComponent<Text>().text = "Lizard: Joined";
                    break;
                case CharacterType.Elephant:
                    _canvas.transform.Find("Elephant").GetComponent<Text>().text = "Elephant: Joined";
                    break;
                case CharacterType.Pig:
                    _canvas.transform.Find("Pig").GetComponent<Text>().text = "Pig: Joined";
                    break;
            }
        }

        return isOK;
    }
}
