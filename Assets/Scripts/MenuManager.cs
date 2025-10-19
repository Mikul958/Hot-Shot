using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void goToMainMenu()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        SceneManager.LoadScene("Main Menu");
    }
    public void goToLevelSelect()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        SceneManager.LoadScene("Level Select");
    }

    public void goToSettings()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        SceneManager.LoadScene("Settings");
    }

    public void exitGame()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        Application.Quit();
    }

    public void goToLevel(string levelName)
    {
        FindObjectOfType<AudioManager>().Play("Button");
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
