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

    void Start() { }

    void Update()
    {
        time += Time.deltaTime;
    }

    public void addStroke()
    {
        strokes++;
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void endLevel()
    {
        
    }
}
