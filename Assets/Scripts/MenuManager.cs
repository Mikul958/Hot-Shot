using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public LevelData levelData;

    public void goToMainMenu()
    {
        AudioManager.instance.Play("Button");
        SceneManager.LoadScene("Main Menu");
    }
    public void goToLevelSelect()
    {
        AudioManager.instance.Play("Button");
        SceneManager.LoadScene("Level Select");
    }

    public void goToSettings()
    {
        AudioManager.instance.Play("Button");
        SceneManager.LoadScene("Settings");
    }

    public void exitGame()
    {
        AudioManager.instance.Play("Button");
        Application.Quit();
    }

    public void goToLevel(int levelNumber)
    {
        AudioManager.instance.Play("Button");
        string levelName = "Level" + levelNumber;
        try
        {
            LevelData.instance.setCurrentLevel(levelNumber);
            SceneManager.LoadScene(levelName);
        }
        catch (Exception e)
        {
            Debug.Log("Error navigating to level \"" + levelName + "\": " + e.Message);
        }
    }
}
