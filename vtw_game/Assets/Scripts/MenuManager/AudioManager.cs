using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource UISource;
    [SerializeField] private VolumeSettings volumeSettings;


    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip PauseMenuOpen;
    public AudioClip PauseMenuClose;
    public AudioClip UISelect;
    public AudioClip UISubmit;
    public AudioClip jump;
    public AudioClip wallGrab;
    public AudioClip edgeClimb;

    #region Singleton
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayUI(AudioClip clip)
    {
        UISource.PlayOneShot(clip);
    }
}
