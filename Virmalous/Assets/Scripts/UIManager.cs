using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject settings;
    public GameObject quitOptions;
    public GameObject quitButton;

    public GameManager game;

    void Resume()
    {
        menu.SetActive(false);
        settings.SetActive(false);
    }

    void OpenSettings()
    {
        menu.SetActive(false);
        settings.SetActive(true);
    }

    void CloseSettings()
    {
        menu.SetActive(true);
        settings.SetActive(false);
    }

    void Quit()
    {
        quitButton.SetActive(false);
        quitOptions.SetActive(true);
    }

    void ToMenu()
    {

    }

    void ToDesktop()
    {
        SaveSystem.SaveGame(game);
        Application.Quit();
    }
}