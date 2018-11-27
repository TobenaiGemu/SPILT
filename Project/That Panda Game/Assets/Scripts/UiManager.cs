using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private GameObject _prevSelected;
    private GameObject _curSelected;
    private bool _dontPlayHighlightSound;

    private SceneManager _sceneManager;

    private Dictionary<int, GameObject> _eventSystems = new Dictionary<int, GameObject>();
    private GameObject _currentEventSystem;
    private GameObject _selectedGameObject;
    [SerializeField]
    private AudioMixer _mainMenuMixer;
    private Slider _volumeSlider;

    private GameScene _gameScene;

    private AudioSource _mainMenuMusic;

    private AudioSource _panSelectSound;
    private AudioSource _hamSelectSound;
    private AudioSource _eliSelectSound;
    private AudioSource _lizSelectSound;
    private AudioSource _highlightSound;
    private AudioSource _selectSound;

    public void Start()
    {
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        _eventSystems.Add(1, GameObject.Find("EventSystem1"));
        _eventSystems.Add(2, GameObject.Find("EventSystem2"));
        _eventSystems.Add(3, GameObject.Find("EventSystem3"));
        _eventSystems.Add(4, GameObject.Find("EventSystem4"));

        _gameScene = GameObject.Find("Scenes").transform.Find("GameScene").GetComponent<GameScene>();
        _volumeSlider = GameObject.Find("Canvas").transform.Find("VolumePanel").transform.Find("Slider").GetComponent<Slider>();

        _mainMenuMusic = GameObject.Find("MainMenuMusic").GetComponent<AudioSource>();
        GameObject menuSounds = GameObject.Find("MenuSounds");
        _panSelectSound = menuSounds.transform.Find("PanSelect").GetComponent<AudioSource>();
        _hamSelectSound = menuSounds.transform.Find("HamSelect").GetComponent<AudioSource>();
        _eliSelectSound = menuSounds.transform.Find("EliSelect").GetComponent<AudioSource>();
        _lizSelectSound = menuSounds.transform.Find("LizSelect").GetComponent<AudioSource>();
        _highlightSound = menuSounds.transform.Find("HighlightButton").GetComponent<AudioSource>();
        _selectSound = menuSounds.transform.Find("SelectButton").GetComponent<AudioSource>();
        ChangeEventSystem(1);

    }

    public void DontPlayFirstHighlightSound()
    {
        _dontPlayHighlightSound = true;
    }

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            _selectedGameObject = EventSystem.current.currentSelectedGameObject;

        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(_selectedGameObject);

        //If the selected button has changed, play the highlight sound
        _prevSelected = _curSelected;
        _curSelected = EventSystem.current.currentSelectedGameObject;
        //Dont play highlight sound is true when the scene has just started and the first button is auto highlighted
        if (_prevSelected != _curSelected && !_dontPlayHighlightSound)
            _highlightSound.Play();
        _dontPlayHighlightSound = false;
    }

    public void ChangeEventSystem(int i)
    {
        foreach (GameObject es in _eventSystems.Values)
            es.SetActive(false);

        _currentEventSystem = _eventSystems[i];
        _currentEventSystem.SetActive(true);
        EventSystem.current = _eventSystems[i].GetComponent<EventSystem>();
    }

    public bool IsHovering(string btnName)
    {
        return (EventSystem.current.currentSelectedGameObject.name == btnName);
    }

    //Every button in the game will call one of these functions
    public void PlayButton()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _selectSound.Play();
        _sceneManager.ChangeScene<JoinGameScene>();
    }

    public void OptionsButton()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _selectSound.Play();
        _sceneManager.ChangeScene<OptionsScene>();
    }

    public void InfoButton()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _selectSound.Play();
        _sceneManager.ChangeScene<InfoScene>();
    }

    public void ResumeGame()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _sceneManager.ResumeScene();
        _gameScene.ResumeGame();
    }

    public void VolumeButton()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _selectSound.Play();
        _sceneManager.GetScene<OptionsScene>().GotoVolume();
    }

    public void ResolutionButton()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _selectSound.Play();
        _sceneManager.GetScene<OptionsScene>().GotoResolution();
    }

    public void OnVolumeChange()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _mainMenuMixer.SetFloat("MainMenuVol", Mathf.Log10(_volumeSlider.value) * 20);
    }

    public void ChangeRes1()
    {
        if (_sceneManager.IsTransitioning())
            return;
        Screen.SetResolution(1024,768,true);
        _sceneManager.GetScene<OptionsScene>().GotoLeftButtons();
    }

    public void ChangeRes2()
    {
        if (_sceneManager.IsTransitioning())
            return;
        Screen.SetResolution(1280, 720, true);
        _sceneManager.GetScene<OptionsScene>().GotoLeftButtons();
    }

    public void ChangeRes3()
    {
        if (_sceneManager.IsTransitioning())
            return;
        Screen.SetResolution(1600, 900, true);
        _sceneManager.GetScene<OptionsScene>().GotoLeftButtons();
    }

    public void ChangeRes4()
    {
        if (_sceneManager.IsTransitioning())
            return;
        Screen.SetResolution(1920, 1080, true);
        _sceneManager.GetScene<OptionsScene>().GotoLeftButtons();
    }

    public void PauseExitGameButton()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _selectSound.Play();
        _sceneManager.ChangeScene<MainMenuScene>();
    }

    public void QuitGameButton()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _selectSound.Play();
        Application.Quit();
    }

    public void SelectPan()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _panSelectSound.Play();
        _sceneManager.GetScene<CharacterSelectScene>().SelectCharacter(CharacterType.Panda);
    }

    public void SelectHam()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _hamSelectSound.Play();
        _sceneManager.GetScene<CharacterSelectScene>().SelectCharacter(CharacterType.Pig);
    }

    public void SelectEli()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _eliSelectSound.Play();
        _sceneManager.GetScene<CharacterSelectScene>().SelectCharacter(CharacterType.Elephant);
    }

    public void SelectLiz()
    {
        if (_sceneManager.IsTransitioning())
            return;
        _lizSelectSound.Play();
        _sceneManager.GetScene<CharacterSelectScene>().SelectCharacter(CharacterType.Lizard);
    }
}
