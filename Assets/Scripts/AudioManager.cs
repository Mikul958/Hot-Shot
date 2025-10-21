using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake () {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        setMusicVolume(SettingsData.instance.musicVolume);
        setSoundVolume(SettingsData.instance.soundVolume);
    }

    void Start ()
    {
        Play("Background_Music");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
    public void UpdateVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.volume = volume;
        s.source.volume = volume;
    }

    public void setMusicVolume(float newVolume)
    {
        UpdateVolume("Background_Music", newVolume);
    }
    public void setSoundVolume(float newVolume)
    {
        UpdateVolume("Rock_bounce", newVolume);
        UpdateVolume("Hole", newVolume);
        UpdateVolume("Button", newVolume);
        UpdateVolume("Putter", newVolume);
        UpdateVolume("Water_splash", newVolume);
        UpdateVolume("Speed_boost", newVolume);
    }
}