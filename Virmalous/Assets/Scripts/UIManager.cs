using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

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

    private List<string> consoleOutput = new List<string>();
    private TMP_Text consoleOutputText;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider speechSlider;
    public Slider sensitivitySlider;

    [Header("References")]
    public GameObject consoleParent;

    [Header("SFX")]
    public AudioSource source;
    public SoundClip[] clips;


    private void Awake()
    {
        consoleOutputText = consoleParent.GetComponentInChildren<TMP_Text>();
    }

    public void AskConsole(string command)
    {
        string output;

        try
        {
            if (command.Contains("allwpns"))
            {
                for (int i = 0; i < game.data.weaponStatus.Count; i++)
                {
                    if (game.data.weaponStatus[i] == false) { game.data.weaponStatus[i] = true; }
                }
                output = "Added all weapons.";
            }
            else if (command.Contains("heal"))
            {
                int amount = int.Parse(command.Split(" ")[1]);
                PlayerScript.ChangeHealth(amount);
                output = $"Increased changed health by: {amount}";
            }
            else if (command.Contains("enchant"))
            {
                BaseWeapon wpn = PlayerScript.cam.GetComponentInChildren<BaseWeapon>();
                string propertyName = command.Split(" ")[1];
                float amount = int.Parse(command.Split(" ")[2]);

                switch (propertyName)
                {
                    case "stun":
                        wpn.damageInfo.stunDamage = amount;
                        break;
                    case "fire":
                        wpn.damageInfo.fireDamage = amount;
                        break;
                    case "freeze":
                        wpn.damageInfo.freezeDamage = amount;
                        break;
                    case "electricity":
                        wpn.damageInfo.electricityDamage = amount;
                        break;
                }

                output = $"Set {propertyName}Damage to: {amount}";
            }else if (command.Contains("load"))
            {
                string sceneName = command.Split(" ")[1];
                SceneManager.LoadScene(sceneName);
                output = $"Loaded scene: {sceneName}";
            }
            else if (command.Contains("timescale"))
            {
                float scale = float.Parse(command.Split(" ")[1]);
                Time.timeScale = scale;
                output = $"Set the timeScale to: {scale}";
            }
            else
            {
                output = "Invalid command.";
            }
        }
        catch
        {
            output = "Invalid command.";
        }

        consoleOutputText.text += ($"\n[{DateTime.Now}] {output}");
    }

    //called from the sliders OnValueChanged() thing and the 1.03 check is there so it ignore the default value when activating
    //for some reason unity calls OnValueChanged() when activated aswell not just when you change the value
    public void UpdateVars()
    {
        if(masterSlider.value != 1.03f) { game.data.MasterVol = masterSlider.value; }
        if(sfxSlider.value != 1.03f) { game.data.SfxVol = sfxSlider.value; }
        if(musicSlider.value != 1.03f) { game.data.MusicVol = musicSlider.value; }
        if(speechSlider.value != 1.03f) { game.data.SpeechVol = speechSlider.value; }

        if(sensitivitySlider.value != 1.03f) { game.data.MouseSensitivity = sensitivitySlider.value; }
    }

    //Call after activating sliders to update them visually (ex. in open control settings at the end)
    void UpdateSliders()
    {
        masterSlider.value = game.data.MasterVol;
        masterSlider.GetComponent<SliderScript>().valueText.text = masterSlider.value.ToString();
        sfxSlider.value = game.data.SfxVol;
        sfxSlider.GetComponent<SliderScript>().valueText.text = sfxSlider.value.ToString();
        musicSlider.value = game.data.MusicVol;
        musicSlider.GetComponent<SliderScript>().valueText.text = musicSlider.value.ToString();
        speechSlider.value = game.data.SpeechVol;
        speechSlider.GetComponent<SliderScript>().valueText.text = speechSlider.value.ToString();

        sensitivitySlider.value = game.data.MouseSensitivity;
        sensitivitySlider.GetComponent<SliderScript>().valueText.text = sensitivitySlider.value.ToString();
    }

    private void Update()
    {
        if(consoleParent.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                TMP_InputField field = consoleParent.GetComponentInChildren<TMP_InputField>();
                field.text = "";
                field.Select();
                field.ActivateInputField();
            }
        }

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

        if (Input.GetKeyDown("`")) 
        {
            TMP_InputField field = consoleParent.GetComponentInChildren<TMP_InputField>();
            field.text = "";
            consoleParent.SetActive(!consoleParent.activeSelf);
            PlayerScript.LockCursor(!consoleParent.activeSelf);
            if (consoleParent.activeSelf) { field.Select(); field.ActivateInputField(); }
            PlayerScript.canMove = !consoleParent.activeSelf;
            //Time.timeScale = consoleParent.activeSelf ? 0f :1f;
        }
    }

    void PlaySound(int index)
    {
        source.PlayOneShot(clips[index].clip, clips[index].volumeMultiplier);
    }

    public void Resume()
    {
        PlaySound(0);
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
        PlaySound(0);
        quitOptions.SetActive(false);
    }

    public void OpenSettings()
    {
        PlaySound(0);
        menu.SetActive(false);
        settings.SetActive(true);
        mainSettings.SetActive(true);
    }

    public void OpenVideoSettings()
    {
        PlaySound(0);
        mainSettings.SetActive(false);
        videoSettings.SetActive(true);
    }

    public void CloseVideoSettings()
    {
        PlaySound(0);
        mainSettings.SetActive(true);
        videoSettings.SetActive(false);
    }

    public void OpenAudioSettings()
    {
        PlaySound(0);
        mainSettings.SetActive(false);
        audioSettings.SetActive(true);

        UpdateSliders();
    }

    public void CloseAudioSettings()
    {
        PlaySound(0);
        mainSettings.SetActive(true);
        audioSettings.SetActive(false);
    }

    public void OpenControlsSettings()
    {
        PlaySound(0);
        mainSettings.SetActive(false);
        controlsSettings.SetActive(true);

        UpdateSliders();
    }

    public void CloseControlsSettings()
    {
        PlaySound(0);
        mainSettings.SetActive(true);
        controlsSettings.SetActive(false);
    }

    public void CloseSettings()
    {
        PlaySound(0);
        menu.SetActive(true);
        settings.SetActive(false);
    }

    public void Quit()
    {
        PlaySound(0);
        //quitButton.SetActive(false);
        quitOptions.SetActive(true);
    }

    public void ToMenu()
    {
        PlaySound(0);
        //load scene
    }

    public void ToDesktop()
    {
        PlaySound(0);
        SaveSystem.SaveGame(game);
        Application.Quit();
    }
}