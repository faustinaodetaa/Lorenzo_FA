using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currResolutionIdx = 0;
        
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currResolutionIdx = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currResolutionIdx;
        resolutionDropdown.RefreshShownValue();
    }

    public void AdjustResolution (int resolutionIdx)
    {
        Resolution resolution = resolutions[resolutionIdx];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void AdjustVolume(float volume)
    {
        //Debug.Log(volume);

        audioMixer.SetFloat("volume", volume);
    }

    public void AdjustGraphics(int graphics)
    {
        QualitySettings.SetQualityLevel(graphics);
    }

    public void AdjustFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
