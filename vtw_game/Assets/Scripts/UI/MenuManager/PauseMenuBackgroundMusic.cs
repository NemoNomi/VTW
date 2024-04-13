using UnityEngine;
using UnityEngine.Audio;

public class PauseMenuBackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup musicMixerGroup;

    private bool isPauseMenuActive = false;
    private bool isAudioSettingsActive = false;

    public void SetPauseMenuActive(bool isActive)
    {
        isPauseMenuActive = isActive;
        UpdateMusicVolume();
    }

    public void SetAudioSettingsActive(bool isActive)
    {
        isAudioSettingsActive = isActive;
        UpdateMusicVolume();
    }

    private void UpdateMusicVolume()
    {
        if (isPauseMenuActive && !isAudioSettingsActive)
        {
            // Apply lowpass filter when the pause menu is active and audio settings are not active
            musicMixerGroup.audioMixer.SetFloat("LowpassCutoff", 360);
            musicMixerGroup.audioMixer.SetFloat("LowpassResonance", 1);
        }
        else
        {
            // Clear the lowpass filter when the pause menu is not active or audio settings are active
            musicMixerGroup.audioMixer.ClearFloat("LowpassCutoff");
            musicMixerGroup.audioMixer.ClearFloat("LowpassResonance");
        }
    }
}
