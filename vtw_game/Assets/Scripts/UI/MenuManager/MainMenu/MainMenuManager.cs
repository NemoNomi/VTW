using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Menu Canvases")]
    [SerializeField] private GameObject MainMenuCanvasGO;
    [SerializeField] private GameObject settingsMenuCanvasGO;
    [SerializeField] private GameObject audiosettingsMenuCanvasGO;

    [Header("First Selected Options")]
    [SerializeField] private GameObject MainMenuFirstSelected;
    [SerializeField] private GameObject settingsMenuFirstSelected;
    [SerializeField] private GameObject audiosettingsMenuFirstSelected;

    #endregion

    private void Start()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        SetMenuVisibility(MainMenuCanvasGO);
        EventSystem.current.SetSelectedGameObject(MainMenuFirstSelected);
    }

    private void SetMenuVisibility(GameObject activeCanvas)
    {
        MainMenuCanvasGO.SetActive(activeCanvas == MainMenuCanvasGO);
        settingsMenuCanvasGO.SetActive(activeCanvas == settingsMenuCanvasGO);
        audiosettingsMenuCanvasGO.SetActive(activeCanvas == audiosettingsMenuCanvasGO);
    }

    #region Menu Navigation
    private void OpenSettingsMenu()
    {
        SetMenuVisibility(settingsMenuCanvasGO);
        EventSystem.current.SetSelectedGameObject(settingsMenuFirstSelected);
    }

    private void OpenAudioSettingsMenu()
    {
        SetMenuVisibility(audiosettingsMenuCanvasGO);
        EventSystem.current.SetSelectedGameObject(audiosettingsMenuFirstSelected);
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

    public void OnSettingsBackPress()
    {
        ShowMainMenu();
    }

    public void OnAudioSettingsBackPress()
    {
        OpenSettingsMenu();
    }

    public void OnPlayButton()
    {
        LoadingScreenManager.Instance.SwitchToScene(1);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
    #endregion
}
