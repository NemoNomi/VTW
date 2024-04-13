using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    #region Assign Canvas
    public GameObject instructionBubble;
    public GameObject instructionOverlay;
    private InputAction interactAction;
    #endregion

    #region Lifecycle
    private void Awake()
    {
        var inputActions = FindObjectOfType<PlayerInput>();
        interactAction = inputActions.actions["Interact"];
        interactAction.performed += HandleInteract;
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
            instructionOverlay.SetActive(false);
        }
    }
    #endregion

    #region Input Action
    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (instructionBubble.activeSelf)
        {
            instructionOverlay.SetActive(!instructionOverlay.activeSelf);
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
