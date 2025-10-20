using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LevelData.instance.loadLevelData();
        SettingsData.instance.loadSettingsData();
        SceneManager.LoadScene("Main Menu");
    }
}
