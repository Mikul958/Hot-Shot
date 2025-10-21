using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Image selectionIndicator;
    

    public void Start()
    {        
        selectionIndicator.rectTransform.anchoredPosition = new Vector2(0, 0);
        
        musicSlider.value = 1f;
        soundSlider.value = 1f;

        bool foundColor = false;
        foreach (SettingsData.SettingsButton button in SettingsData.instance.settingsButtons)
        {
            if (SettingsData.instance.ballColor == button.color)
            {
                selectionIndicator.rectTransform.anchoredPosition = new Vector2(button.xPosition, 0);
                foundColor = true;
                break;
            }
        }
        if (!foundColor)
        {
            SettingsData.instance.ballColor = "FFFFFFFF";
            selectionIndicator.rectTransform.anchoredPosition = new Vector2(-260, 0);
        }
    }

    public void OnBallColorClicked(int buttonIndex)
    {
        AudioManager.instance.Play("Button");
        SettingsData.instance.ballColor = SettingsData.instance.settingsButtons[buttonIndex].color;
        selectionIndicator.rectTransform.anchoredPosition = new Vector2(SettingsData.instance.settingsButtons[buttonIndex].xPosition, 0);
    }
    
    public void OnMusicSliderChanged(float newVolume)
    {
        SettingsData.instance.musicVolume = newVolume;
        AudioManager.instance.UpdateVolume("Background_Music", newVolume);
    }

    public void OnSFXSliderChanged(float newVolume)
    {
        SettingsData.instance.soundVolume = newVolume;
        AudioManager.instance.UpdateVolume("Rock_bounce", newVolume);
        AudioManager.instance.UpdateVolume("Hole", newVolume);
        AudioManager.instance.UpdateVolume("Button", newVolume);
        AudioManager.instance.UpdateVolume("Putter", newVolume);
        AudioManager.instance.UpdateVolume("Water_splash", newVolume);
        AudioManager.instance.UpdateVolume("Speed_boost", newVolume);
    }

    public void triggerSettingsSave()
    {
        SettingsData.instance.saveSettingsData();
    }
}
