using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Pause Menu Canvas")]
    [SerializeField] private GameObject pauseMenuCanvasGO;
    [SerializeField] private GameObject settingsMenuCanvasGO;

    [Header("First Selected Options")]
    [SerializeField] private GameObject pauseMenuFirstSelected;
    [SerializeField] private GameObject settingsMenuFirstSelected;
    #endregion

    #region Private Fields
    private bool isPaused = false;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeMenus();
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
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        CloseAllMenus();
        PlayerActionMapManager.instance.SwitchActionMapsToGameplay();
    }
    #endregion

    #region Menu Visibility
    private void SetMenuVisibility(bool isVisible)
    {
        pauseMenuCanvasGO.SetActive(isVisible);
        settingsMenuCanvasGO.SetActive(isVisible);
    }

    private void OpenPauseMenu()
    {
        SetMenuVisibility(false);
        pauseMenuCanvasGO.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseMenuFirstSelected);
    }

    private void OpenSettingsMenuHandle()
    {
        SetMenuVisibility(false);
        settingsMenuCanvasGO.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsMenuFirstSelected);
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
        OpenSettingsMenuHandle();
    }

    public void OnResumePress()
    {
        ResumeGame();
    }

    public void OnSettingsBackPress()
    {
        OpenPauseMenu();
    }
    #endregion
}
