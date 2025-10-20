using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "Scriptable Objects/SettingsData")]
public class SettingsData : ScriptableObject
{
    private String settingsPath;
    
    [SerializeField] private float _musicVolume;
    public float musicVolume
    {
        get => _musicVolume;
        set { updateMusicVolume(value); }
    }
    
    [SerializeField] private float _soundVolume;
    public float soundVolume
    {
        get => _soundVolume;
        set { updateSFXVolume(value); }
    }

    [SerializeField] private string _ballColor;
    public string ballColor { get => _ballColor; set => _ballColor = value; }

    void OnEnable()
    {
        _musicVolume = 1f;
        _soundVolume = 1f;
        _ballColor = "#FFFFFF";
        settingsPath = Path.Combine(Application.persistentDataPath, "settings.json");
    }

    public void loadSettingsData()
    {
        if (!File.Exists(settingsPath))
        {
            saveSettingsData();
            return;
        }

        string settingsJSON = File.ReadAllText(settingsPath);
        JsonUtility.FromJsonOverwrite(settingsJSON, this);

        Debug.Log($"Loaded Settings: {_musicVolume}, {_soundVolume}, {_ballColor}");
    }

    public void saveSettingsData()
    {
        string settingsJSON = JsonUtility.ToJson(this, true);
        File.WriteAllText(settingsPath, settingsJSON);
    }

    private void updateMusicVolume(float value)
    {
        _musicVolume = value;
        if (_musicVolume < 0f)
            _musicVolume = 0f;
        else if (_musicVolume > 1f)
            _musicVolume = 1f;

        Debug.Log("Music volume updated to " + _musicVolume); // TODO audio mixer?
    }

    private void updateSFXVolume(float value)
    {
        _soundVolume = value;
        if (_soundVolume < 0f)
            _soundVolume = 0f;
        else if (_soundVolume > 1f)
            _soundVolume = 1f;
        
        Debug.Log("Sound volume updated to " + _soundVolume); // TODO audio mixer?
    }
}
