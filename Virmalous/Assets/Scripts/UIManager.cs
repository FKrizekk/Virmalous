using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject settings;
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
                settings.SetActive(false);
                quitOptions.SetActive(false);
                PlayerScript.LockCursor(true);
                Time.timeScale = 1f;
            }
            else if (menu.activeSelf)
            {
                menu.SetActive(false);
                quitOptions.SetActive(false);
                PlayerScript.LockCursor(true);
                Time.timeScale = 1f;
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
        menu.SetActive(false);
        settings.SetActive(false);
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