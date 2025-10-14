using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    // Referenced game objects / components
    public GameObject pauseMenu;
    public GameObject levelCompleteMenu;
    public GameObject levelFailedMenu;
    public GameObject backgroundDim;
    public TextMeshProUGUI strokeText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI parText;

    // Level-specific constants, set in this component in game engine
    public int par;             // Par for the current level -- par awards one star, birdie awards 2
    public float timeToBeat;    // Time to beat for the player to earn another star

    // Instance variables
    private int strokes = 0;
    private float time = 0f;
    private bool pauseTimers = true;
    short starMask;

    void Start()
    {
        strokeText.text = "Strokes: " + strokes;
        timeText.text = TimeSpan.FromSeconds((int)time).ToString(@"mm\:ss");
        parText.text = "Par " + par;
    }

    void Update()
    {
        if (!pauseTimers)
        {
            time += Time.deltaTime;
            timeText.text = TimeSpan.FromSeconds((int)time).ToString(@"mm\:ss");
        }
    }

    public void addStroke()
    {
        strokes++;
        strokeText.text = "Strokes: " + strokes;
        pauseTimers = false;    // Remove timer pause from start of level
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
        FindObjectOfType<AudioManager>().Play("Button");
        backgroundDim.SetActive(true);
        pauseMenu.SetActive(true);
    }

    public void resumeGame()
    {
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().Play("Button");
        backgroundDim.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void exitLevel()
    {
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().Play("Button");
        SceneManager.LoadScene("Level Select");
    }

    public void restartLevel()
    {
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().Play("Button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // TODO can totally restart without reloading scene but lazy
    }

    public void nextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        FindObjectOfType<AudioManager>().Play("Button");
        if (sceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(sceneIndex + 1);
        else
            exitLevel();
    }

    public void endLevel()
    {
        pauseTimers = true;
        if (strokes > par + GameConfig.instance.allowedOverPar)
        {
            showFailedMenu();
            return;
        }

        starMask = GameConfig.NO_STAR_MASK;
        if (strokes <= par)
        {
            starMask += GameConfig.FIRST_STAR_MASK;
            if (strokes != par)
                starMask += GameConfig.SECOND_STAR_MASK;
        }
        if (time <= timeToBeat)
        {
            starMask += GameConfig.THIRD_STAR_MASK;
        }

        // TODO init level complete screen and send starMask in event?
        showCompleteMenu(starMask);
    }

    public void showCompleteMenu(short starMask)
    {
        // TODO implement stars in object and code
        backgroundDim.SetActive(true);
        levelCompleteMenu.SetActive(true);
    }

    public void showFailedMenu()
    {
        backgroundDim.SetActive(true);
        levelFailedMenu.SetActive(true);
    }
}
