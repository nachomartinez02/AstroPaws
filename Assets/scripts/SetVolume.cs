using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public void SetVolumen (float volume)
    {
        bool result = audioMixer.SetFloat("Volume", volume);
        if (!result)
        {
            Debug.LogError("Failed to set volume. Ensure the 'Volume' parameter exists in the AudioMixer.");
        }
    }

    public void SetQuality (int qualityIndex)
    {
        if (qualityIndex >= 0 && qualityIndex < QualitySettings.names.Length)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            Debug.Log($"Quality level set to: {qualityIndex} - {QualitySettings.names[qualityIndex]}");
        }
        else
        {
            Debug.LogError($"Invalid quality index: {qualityIndex}. It must be between 0 and {QualitySettings.names.Length - 1}.");
        }
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}