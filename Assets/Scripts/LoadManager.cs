using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public LevelData levelData;
    public SettingsData settingsData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelData.loadLevelData();
        settingsData.loadSettingsData();
        SceneManager.LoadScene("Main Menu");
    }
}
