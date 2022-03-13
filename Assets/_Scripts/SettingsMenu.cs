using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider FXSlider;
    public Toggle fullscreenToggle;
    public Toggle rumbleToggle;
    public Dropdown resolutionDropdown;
    public Rumbler rumble;
    Resolution[] resolutions;

    private void Start()
    {
        // Set current fullscreen setting
        fullscreenToggle.isOn = Screen.fullScreen;

        // Set current rumble setting
        rumbleToggle.isOn = rumble.rumbleOn;

        // Set current volume level
        masterMixer.GetFloat("MasterVolume", out float masterVolume);
        masterVolume = Mathf.Pow (10f ,(masterVolume / 20f));
        masterSlider.value = masterVolume;

        masterMixer.GetFloat("MusicVolume", out float musicVolume);
        musicVolume = Mathf.Pow(10f, (musicVolume / 20f));
        musicSlider.value = musicVolume;

        masterMixer.GetFloat("FXVolume", out float FXVolume);
        FXVolume = Mathf.Pow(10f, (FXVolume / 20f));
        FXSlider.value = FXVolume;

        // set current resolution
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i =0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMasterVolume (float volume)
    {
        float logMasterVolume = Mathf.Log10(volume) * 20f;
        masterMixer.SetFloat("MasterVolume", logMasterVolume);
    }

    public void SetMusicVolume(float volume)
    {
        float logMusicVolume = Mathf.Log10(volume) * 20f;
        masterMixer.SetFloat("MusicVolume", logMusicVolume);
    }

    public void SetFXVolume(float volume)
    {
        float logFXVolume = Mathf.Log10(volume) * 20f;
        masterMixer.SetFloat("FXVolume", logFXVolume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetRumble(bool rumbleOn)
    {
        rumble.rumbleOn = rumbleOn;
    }
}
