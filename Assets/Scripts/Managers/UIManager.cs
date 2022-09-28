using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameObject _player;
    [SerializeField]
    private Text _healthText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject deathMenu;
    [SerializeField]
    private GameObject winMenu;

    [SerializeField]
    private Slider VolumeSlider;
    [SerializeField]
    private Toggle FullScreenToggle;

    public GameObject MainFirstButton;
    public GameObject optionsFirstButton;
    public GameObject optionsCloseButton;
    public GameObject deathMenuButton;
    public GameObject winMenuButton;

    public GameObject[] _quotes;

    void Start()
    {
        _player = GameObject.Find("Player");
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1);
        }
        else
        {
            Load();
        }
    }
   public void Unpause()
    {
        pauseMenu.SetActive(false);
        _player.GetComponent<PlayerController>().UnfreezeTime();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
    }
    public void ChangeVolume()
    {
        AudioListener.volume = VolumeSlider.value;
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("Volume", VolumeSlider.value);

    }
    public void Load()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }
    public void ChangeScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SettingsPanel()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }
    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsCloseButton);
    }
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void DeathMenu()
    {
        deathMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(deathMenuButton);
    }  

    public void WinMenu()
    {
        winMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(winMenuButton);
    }
    
    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

}
