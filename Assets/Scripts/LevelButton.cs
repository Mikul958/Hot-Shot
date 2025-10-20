using UnityEditor;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int levelNumber;
    private MenuManager menuManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuManager = FindFirstObjectByType<MenuManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void clickLevelButton()
    {
        menuManager.goToLevel(levelNumber);
    }
}
