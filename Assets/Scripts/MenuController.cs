using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public GameObject _settingsMenu;
    public GameObject _mainMenu;

    public GameObject MainFirstButton;
    public GameObject optionsFirstButton;
    public GameObject optionsCloseButton;

    [SerializeField]
    private Slider VolumeSlider;
    [SerializeField]
    private Toggle FullScreenToggle;

    public void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1);
        }
        else
        {
            Load();
        }
    }
    public void PlayGame()
  {
      SceneManager.LoadScene(1);
  }

    public void SettingsMenu()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        
    }

    public void CloseSettingsMenu()
    {
        _settingsMenu.SetActive(false);
        _mainMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsCloseButton);
    }

   public void QuitGame()
  {
      Application.Quit();
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
}
