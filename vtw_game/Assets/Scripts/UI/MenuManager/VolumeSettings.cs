using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class VolumeSettings : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider UISlider;


   
    #endregion


    #region Set Default Values
    private void Start()
    {
        SetDefaultVolumeValues();
    }
    #endregion


    #region Setting Audio Volumes
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }


    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }


    public void SetUIVolume()
    {
        float volume = UISlider.value;
        myMixer.SetFloat("UI", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("UIVolume", volume);
    }


    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    #endregion


    #region Loading PlayerPrefs Volumes
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        UISlider.value = PlayerPrefs.GetFloat("UIVolume");
        SetMusicVolume();
        SetSFXVolume();
        SetMasterVolume();
        SetUIVolume();
    }
    #endregion


    #region Loading Default Volumes
    private void SetDefaultVolumeValues()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.75f);
        }
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 0.75f);
        }
        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume", 0.75f);
        }
        if (!PlayerPrefs.HasKey("UIVolume"))
        {
            PlayerPrefs.SetFloat("UIVolume", 0.75f);
        }


        LoadVolume();
    }
    #endregion
}



