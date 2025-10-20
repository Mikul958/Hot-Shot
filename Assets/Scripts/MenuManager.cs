using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public LevelData levelData;
    public SettingsData settingsData;
    private AudioManager audioManager;

    void Start()
    {
        Debug.Log("Attempting to find audio manager");
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
        if (audioManager == null)
            Debug.Log("Audio Manager is null");
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

    public void OnMusicSliderChanged(float newVolume)
    {
        settingsData.musicVolume = newVolume;
    }

    public void OnSFXSliderChanged(float newVolume)
    {
        settingsData.soundVolume = newVolume;
    }

    public void triggerSettingsSave()
    {
        settingsData.saveSettingsData();
    }
}
