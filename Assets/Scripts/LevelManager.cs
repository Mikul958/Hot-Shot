using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Referenced components
    // TODO

    // Level-specific constants, set in this component in game engine
    public int par;             // Par for the current level -- par awards one star, birdie awards 2
    public float timeToBeat;    // Time to beat for the player to earn another star

    // Instance variables
    private int strokes = 0;
    private float time = 0f;
    private bool pauseTimers = true;

    void Start() { }

    void Update()
    {
        if (!pauseTimers)
            time += Time.deltaTime;
    }

    public void addStroke()
    {
        strokes++;
        pauseTimers = false;
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // TODO can totally restart without reloading scene but lazy
    }

    public void endLevel()
    {
        pauseTimers = true;
        if (strokes > par + GameConfig.instance.allowedOverPar)
        {
            // TODO init level failed screen
            Debug.Log("Level Failed...");
            return;
        }

        short starMask = GameConfig.NO_STAR_MASK;
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
        Debug.Log($"Level Complete!\nStrokes: {strokes}, Time: {time} seconds\nPar: {(starMask & GameConfig.FIRST_STAR_MASK) > 0}, Birdie: {(starMask & GameConfig.SECOND_STAR_MASK) > 0}, Beat Time: {(starMask & GameConfig.THIRD_STAR_MASK) > 0}");
    }
}
