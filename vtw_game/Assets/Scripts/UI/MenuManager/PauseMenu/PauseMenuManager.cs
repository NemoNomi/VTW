using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Pause Menu Canvas")]
    [SerializeField] private GameObject pauseMenuCanvasGO;
    [SerializeField] private GameObject settingsMenuCanvasGO;
    [SerializeField] private GameObject audiosettingsMenuCanvasGO;
    [SerializeField] private GameObject controlssettingsMenuCanvasGO;

    [Header("First Selected Options")]
    [SerializeField] private GameObject pauseMenuFirstSelected;
    [SerializeField] private GameObject settingsMenuFirstSelected;
    [SerializeField] private GameObject audiosettingsMenuFirstSelected;
    [SerializeField] private GameObject controlssettingsMenuFirstSelected;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip openMenuSound;
    [SerializeField] private AudioClip closeMenuSound;
    #endregion

    #region Private Fields
    private bool isPaused = false;
    private PauseMenuBackgroundMusic pauseMenuBackgroundMusic;
    private AudioSource audioSource;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeMenus();
        pauseMenuBackgroundMusic = FindObjectOfType<PauseMenuBackgroundMusic>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandlePauseInput();
    }
    #endregion

    #region Initialization
    private void InitializeMenus()
    {
        SetMenuVisibility(false);
    }
    #endregion

    #region Input Handling
    private void HandlePauseInput()
    {
        if (GamePauseInputManager.instance != null && GamePauseInputManager.instance.PauseMenuOpenCloseInput)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }
    #endregion

    #region Game Pause/Resume
    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        OpenPauseMenu();
        PlayerActionMapManager.instance.SwitchActionMapsToUI();
        pauseMenuBackgroundMusic.SetPauseMenuActive(true);
        AudioManager.instance.PlayUI(openMenuSound);
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        CloseAllMenus();
        PlayerActionMapManager.instance.SwitchActionMapsToGameplay();
        pauseMenuBackgroundMusic.SetPauseMenuActive(false);
        AudioManager.instance.PlayUI(closeMenuSound);
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }
    #endregion

    #region Menu Visibility
    private void SetMenuVisibility(bool isVisible)
    {
        pauseMenuCanvasGO.SetActive(isVisible);
        settingsMenuCanvasGO.SetActive(isVisible);
        audiosettingsMenuCanvasGO.SetActive(isVisible);
        controlssettingsMenuCanvasGO.SetActive(isVisible);
    }

    private void OpenPauseMenu()
    {
        SetMenuVisibility(false);
        pauseMenuCanvasGO.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseMenuFirstSelected);
    }

    private void OpenSettingsMenu()
    {
        SetMenuVisibility(false);
        settingsMenuCanvasGO.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsMenuFirstSelected);
    }
    private void OpenControlsSettingsMenu()
    {
        SetMenuVisibility(false);
        controlssettingsMenuCanvasGO.SetActive(true);
        EventSystem.current.SetSelectedGameObject(controlssettingsMenuFirstSelected);
    }

    private void OpenAudioSettingsMenu()
    {
        SetMenuVisibility(false);
        audiosettingsMenuCanvasGO.SetActive(true);
        EventSystem.current.SetSelectedGameObject(audiosettingsMenuFirstSelected);
        pauseMenuBackgroundMusic.SetAudioSettingsActive(true);
    }

    private void CloseAllMenus()
    {
        SetMenuVisibility(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
    #endregion


    #region UI Button Handlers
    public void OnSettingsPress()
    {
        OpenSettingsMenu();
    }
    public void OnMainMenuPress()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void OnAudioSettingsPress()
    {
        OpenAudioSettingsMenu();
    }
    public void OnControlsSettingsPress()
    {
        OpenControlsSettingsMenu();
    }

    public void OnResumePress()
    {
        ResumeGame();
    }

    public void OnSettingsBackPress()
    {
        OpenPauseMenu();
    }
    public void OnControlsBackPress()
    {
        OpenSettingsMenu();
    }
    public void OnAudioSettingsBackPress()
    {
        OpenSettingsMenu();
        pauseMenuBackgroundMusic.SetAudioSettingsActive(false);
    }
    #endregion
}
