using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public SettingsData settingsData;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Image selectionIndicator;
    

    public void Start()
    {        
        selectionIndicator.rectTransform.anchoredPosition = new Vector2(0, 0);
        
        musicSlider.value = settingsData.musicVolume;
        soundSlider.value = settingsData.soundVolume;

        bool foundColor = false;
        foreach (SettingsData.SettingsButton button in settingsData.settingsButtons)
        {
            if (settingsData.ballColor == button.color)
            {
                selectionIndicator.rectTransform.anchoredPosition = new Vector2(button.xPosition, 0);
                foundColor = true;
                break;
            }
        }
        if (!foundColor)
        {
            settingsData.ballColor = "FFFFFFFF";
            selectionIndicator.rectTransform.anchoredPosition = new Vector2(-260, 0);
        }
    }

    public void OnBallColorClicked(int buttonIndex)
    {
        settingsData.ballColor = settingsData.settingsButtons[buttonIndex].color;
        selectionIndicator.rectTransform.anchoredPosition = new Vector2(settingsData.settingsButtons[buttonIndex].xPosition, 0);
    }
    
    public void OnMusicSliderChanged(float newVolume)
    {
        settingsData.musicVolume = newVolume;
    }

    public void OnSFXSliderChanged(float newVolume)
    {
        settingsData.soundVolume = newVolume;
    }

    public void triggerSettingsSave()
    {
        settingsData.saveSettingsData();
    }
}
