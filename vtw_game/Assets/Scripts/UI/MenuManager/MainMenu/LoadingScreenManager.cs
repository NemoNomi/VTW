using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    #region Singleton
    public static LoadingScreenManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    #region Serialized Fields
    public GameObject LoadingScreenObject;
    public Slider ProgressBar;
    public CanvasGroup transitionPanelCanvasGroup;
    public TransitionManager transitionManager;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float progressSpeed = 0.5f;
    [SerializeField] private float minimumDisplayTime = 3f;
    #endregion

    #region Scene Loading
    public void SwitchToScene(int id)
    {
        SetupLoadingScreen();
        StartCoroutine(SwitchToSceneAsync(id));
    }

    private void SetupLoadingScreen()
    {
        LoadingScreenObject.SetActive(true);
        ProgressBar.value = 0;
    }

    private IEnumerator SwitchToSceneAsync(int id)
    {
        SetupLoadingScreen();
        yield return StartCoroutine(PerformSceneLoadAsync(id));
    }

    private AsyncOperation StartSceneLoad(int id)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id);
        asyncLoad.allowSceneActivation = false;
        return asyncLoad;
    }

    private IEnumerator PerformSceneLoadAsync(int id)
    {
        AsyncOperation asyncLoad = StartSceneLoad(id);

        while (!asyncLoad.isDone)
        {
            yield return MonitorProgress(asyncLoad);
            if (ShouldTriggerTransitions())
            {
                yield return TriggerTransitions(asyncLoad);
                break;
            }
        }
    }

    private IEnumerator MonitorProgress(AsyncOperation asyncLoad)
    {
        while (!IsMinimumDisplayTimeElapsed() || !IsProgressOverThreshold(asyncLoad))
        {
            yield return null;
        }
    }
    #endregion

    #region Transition Methods
    private IEnumerator TriggerTransitions(AsyncOperation asyncLoad)
    {
        yield return StartCoroutine(transitionManager.FadeToBlack(fadeDuration, transitionPanelCanvasGroup));
        LoadingScreenObject.SetActive(false);
        asyncLoad.allowSceneActivation = true;
        yield return new WaitForSeconds(0.01f);
        yield return StartCoroutine(transitionManager.FadeFromBlack(fadeDuration, transitionPanelCanvasGroup));
    }
    #endregion

    #region Helper Methods
    private bool IsMinimumDisplayTimeElapsed()
    {
        return Time.time >= minimumDisplayTime;
    }

    private bool IsProgressOverThreshold(AsyncOperation asyncLoad)
    {
        float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
        ProgressBar.value = Mathf.Lerp(ProgressBar.value, progress, progressSpeed * Time.deltaTime);
        return ProgressBar.value >= 0.99f;
    }

    private bool ShouldTriggerTransitions()
    {
        return IsMinimumDisplayTimeElapsed() && ProgressBar.value >= 0.99f;
    }
    #endregion
}
