using UnityEngine;
using System.Collections;

public class InteractionControllerBegin : MonoBehaviour
{
    #region Assign Canvas and AudioSource
    public GameObject instructionBubble;
    public PauseMenuManager pauseMenuManager;
    public float initialDelay = 0.5f;
    private bool hasShownOnce = false;
    private AudioSource audioSource;
    private bool wasGamePaused = false;
    #endregion

    #region Lifecycle
    private void Start()
    {
        instructionBubble.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (pauseMenuManager != null && pauseMenuManager.IsGamePaused())
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                wasGamePaused = true;
            }
        }
        else if (wasGamePaused)
        {
            audioSource.Play();
            wasGamePaused = false;
        }
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasShownOnce)
            {
                StartCoroutine(ShowInstructionAfterDelay());
            }
            else
            {
                instructionBubble.SetActive(true);
                if (!pauseMenuManager.IsGamePaused())
                {
                    audioSource.Play();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            instructionBubble.SetActive(false);
            audioSource.Stop();
        }
    }
    #endregion

    #region Coroutine
    private IEnumerator ShowInstructionAfterDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        instructionBubble.SetActive(true);
        if (!pauseMenuManager.IsGamePaused())
        {
            audioSource.Play();
        }
        hasShownOnce = true;
    }
    #endregion
}
