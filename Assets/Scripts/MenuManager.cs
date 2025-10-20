using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    public void goToMainMenu()
    {
        audioManager.Play("Button");
        SceneManager.LoadScene("Main Menu");
    }
    public void goToLevelSelect()
    {
        audioManager.Play("Button");
        SceneManager.LoadScene("Level Select");
    }

    public void goToSettings()
    {
        audioManager.Play("Button");
        SceneManager.LoadScene("Settings");
    }

    public void exitGame()
    {
        audioManager.Play("Button");
        Application.Quit();
    }

    public void goToLevel(string levelName)
    {
        audioManager.Play("Button");
        try
        {
            SceneManager.LoadScene(levelName);
        }
        catch (Exception e)
        {
            Debug.Log("Error navigating to level \"" + levelName + "\": " + e.Message);
        }
    }
}
