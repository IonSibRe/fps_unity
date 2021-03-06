using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    // TODO: Fix default graphics settings.

    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;

    Resolution[] resolutions;

    void Start()
    {
        UpdateResolutionDropdown();
        UpdateGraphicsDropdown();
    }

    public void SetResolution(int resolutionIndex) 
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
        
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetSensitivity(float sensitivity)
    {
        MouseLook.mouseSens = sensitivity;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    private void UpdateResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void UpdateGraphicsDropdown()
    {
        qualityDropdown.ClearOptions();

        List<string> qualitySettings = new List<string>(QualitySettings.names);
        int currentQualityIndex = 0;

        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            if (QualitySettings.GetQualityLevel() == i)
            {
                currentQualityIndex = i;
            }
        }

        qualityDropdown.AddOptions(qualitySettings);
        qualityDropdown.value = currentQualityIndex;
        qualityDropdown.RefreshShownValue();
    }

}
