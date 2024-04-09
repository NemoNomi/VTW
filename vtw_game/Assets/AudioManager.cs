using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] private VolumeSettings volumeSettings;


    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip PauseMenuOpen;
    public AudioClip PauseMenuClose;
    public AudioClip UISelect;

private void Start()
{
    musicSource.clip = background;
    musicSource.Play();
}

public void PlaySFX(AudioClip clip)
{
    SFXSource.PlayOneShot(clip);
}
}
