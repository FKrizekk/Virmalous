using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject settings;
    public GameObject mainSettings;
    public GameObject videoSettings;
    public GameObject audioSettings;
    public GameObject controlsSettings;
    public GameObject quitOptions;
    public GameObject quitButton;
    public GameObject resumeButton;
    public GameObject settingsButton;

    public GameManager game;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider speechSlider;
    public Slider sensitivitySlider;

    public void UpdateVars()
    {
        game.data.MasterVol = masterSlider.value;
        game.data.SfxVol = sfxSlider.value;
        game.data.MusicVol = musicSlider.value;
        game.data.SpeechVol = speechSlider.value;

        game.data.MouseSensitivity = sensitivitySlider.value;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menu.activeSelf && settings.activeSelf)
            {
                Resume();
            }
            else if (menu.activeSelf)
            {
                Resume();
            }
            else if (!menu.activeSelf)
            {
                menu.SetActive(true);
                PlayerScript.LockCursor(false);
                Time.timeScale = 0f;
            }
        }
    }

    public void Resume()
    {
        settings.SetActive(false);
        quitOptions.SetActive(false);
        mainSettings.SetActive(false);
        videoSettings.SetActive(false);
        audioSettings.SetActive(false);
        controlsSettings.SetActive(false);
        menu.SetActive(false);

        SaveSystem.SaveGame(game);

        PlayerScript.LockCursor(true);
        Time.timeScale = 1f;
    }

    public void StopQuitOptions()
    {
        quitOptions.SetActive(false);
    }

    public void OpenSettings()
    {
        menu.SetActive(false);
        settings.SetActive(true);
        mainSettings.SetActive(true);
    }

    public void OpenVideoSettings()
    {
        mainSettings.SetActive(false);
        videoSettings.SetActive(true);
    }

    public void CloseVideoSettings()
    {
        mainSettings.SetActive(true);
        videoSettings.SetActive(false);
    }

    public void OpenAudioSettings()
    {
        mainSettings.SetActive(false);
        audioSettings.SetActive(true);

        masterSlider.value = game.data.MasterVol;
        sfxSlider.value = game.data.SfxVol;
        musicSlider.value = game.data.MusicVol;
        speechSlider.value = game.data.SpeechVol;
    }

    public void CloseAudioSettings()
    {
        mainSettings.SetActive(true);
        audioSettings.SetActive(false);
    }

    public void OpenControlsSettings()
    {
        mainSettings.SetActive(false);
        controlsSettings.SetActive(true);

        sensitivitySlider.value = game.data.MouseSensitivity;
    }

    public void CloseControlsSettings()
    {
        mainSettings.SetActive(true);
        controlsSettings.SetActive(false);
    }

    public void CloseSettings()
    {
        menu.SetActive(true);
        settings.SetActive(false);
    }

    public void Quit()
    {
        //quitButton.SetActive(false);
        quitOptions.SetActive(true);
    }

    public void ToMenu()
    {
        //load scene
    }

    public void ToDesktop()
    {
        SaveSystem.SaveGame(game);
        Application.Quit();
    }
}