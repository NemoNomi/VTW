using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController2CanvasAudio : MonoBehaviour
{
    #region Assign Canvas and AudioSource
    public GameObject instructionBubble;
    public GameObject instructionOverlay;
    public PauseMenuManager pauseMenuManager;
    private InputAction interactAction;
    private AudioSource audioSource;
    private bool wasGamePaused = false;
    #endregion

    #region Lifecycle
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the game object!");
        }

        var inputActions = FindObjectOfType<PlayerInput>();
        interactAction = inputActions.actions["Interact"];
        interactAction.performed += HandleInteract;
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
        else if (wasGamePaused && !audioSource.isPlaying && instructionOverlay.activeSelf)
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
            instructionBubble.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            instructionBubble.SetActive(false);
            if (instructionOverlay.activeSelf)
            {
                instructionOverlay.SetActive(false);
                audioSource.Stop();
            }
        }
    }
    #endregion

    #region Input Action
    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (instructionBubble.activeSelf)
        {
            instructionOverlay.SetActive(!instructionOverlay.activeSelf);

            if (instructionOverlay.activeSelf && !pauseMenuManager.IsGamePaused())
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
    #endregion

    #region Destroy
    private void OnDestroy()
    {
        interactAction.performed -= HandleInteract;
    }
    #endregion
}
