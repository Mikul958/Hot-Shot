using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void goToMainMenu()
    {
        // TODO play sound
        SceneManager.LoadScene("Main Menu");
    }
    public void goToLevelSelect()
    {
        // TODO play sound
        SceneManager.LoadScene("Level Select");
    }

    public void goToSettings()
    {
        // TODO play sound
        SceneManager.LoadScene("Settings");
    }

    public void exitGame()
    {
        // TODO play sound
        Application.Quit();
    }

    public void goToLevel(string levelName)
    {
        // TODO play sound
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
