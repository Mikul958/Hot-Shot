using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "Scriptable Objects/SettingsData")]
public class SettingsData : ScriptableObject
{
    private static SettingsData _instance;
    public static SettingsData instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<SettingsData>("SettingsData");
            return _instance;
        }
    }

    public class SettingsButton
    {
        public float xPosition;
        public string color;

        public SettingsButton(float xPosition, string color)
        {
            this.xPosition = xPosition;
            this.color = color;
        }
    }

    private String settingsPath;

    // This is some horrific code LOL trust me I was low on time
    public SettingsButton[] settingsButtons = {
        new SettingsButton(-260, "#FFFFFFFF"),
        new SettingsButton(-130, "#DD0909FF"),
        new SettingsButton(0, "#3651FFFF"),
        new SettingsButton(130, "#119F00FF"),
        new SettingsButton(260, "#8B06FDFF")
    };
    
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
        loadSettingsData();
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
