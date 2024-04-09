using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Pause Menu Canvas")]
    [SerializeField] private GameObject pauseMenuCanvasGO;
    [SerializeField] private GameObject settingsMenuCanvasGO;
    [SerializeField] private GameObject audiosettingsMenuCanvasGO;

    [Header("First Selected Options")]
    [SerializeField] private GameObject pauseMenuFirstSelected;
    [SerializeField] private GameObject settingsMenuFirstSelected;
    [SerializeField] private GameObject audiosettingsMenuFirstSelected;

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
        PlaySoundEffect(openMenuSound);
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        CloseAllMenus();
        PlayerActionMapManager.instance.SwitchActionMapsToGameplay();
        pauseMenuBackgroundMusic.SetPauseMenuActive(false);
        PlaySoundEffect(closeMenuSound);
    }
    #endregion

    #region Menu Visibility
    private void SetMenuVisibility(bool isVisible)
    {
        pauseMenuCanvasGO.SetActive(isVisible);
        settingsMenuCanvasGO.SetActive(isVisible);
        audiosettingsMenuCanvasGO.SetActive(isVisible);
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

        #region Sound Effects
    private void PlaySoundEffect(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    #endregion

    #region UI Button Handlers
    public void OnSettingsPress()
    {
        OpenSettingsMenu();
    }

    public void OnAudioSettingsPress()
    {
        OpenAudioSettingsMenu();
    }

    public void OnResumePress()
    {
        ResumeGame();
    }

    public void OnSettingsBackPress()
    {
        OpenPauseMenu();
    }

    public void OnAudioSettingsBackPress()
    {
        OpenSettingsMenu();
        pauseMenuBackgroundMusic.SetAudioSettingsActive(false);
    }
    #endregion
}
