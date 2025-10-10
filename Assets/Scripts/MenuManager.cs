using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void openMainMenu()
    {
        // TODO play sound
        SceneManager.LoadScene("Main Menu");
    }
    public void openLevelSelect()
    {
        // TODO play sound
        SceneManager.LoadScene("Level Select");
    }

    public void openSettings()
    {
        // TODO play sound
        SceneManager.LoadScene("Settings");
    }

    public void exitGame()
    {
        // TODO play sound
        Application.Quit();
    }

    public void goTolevel(int level)
    {
        // TODO play sound
        try
        {
            SceneManager.LoadScene("Level" + level);
        }
        catch (Exception e)
        {
            Debug.Log("Error navigating to level " + level + ": " + e.Message);
        }
    }
}
