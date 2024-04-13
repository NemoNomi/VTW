using UnityEngine;
using UnityEngine.InputSystem;

public class GamePauseInputManager : MonoBehaviour
{
    #region Singleton Pattern
    public static GamePauseInputManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }    
    #endregion

    #region Private Fields
    private PlayerInput _playerInput;
    private InputAction _pauseMenuOpenCloseAction;
    #endregion

    #region Public Properties
    public bool PauseMenuOpenCloseInput { get; private set; }
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _pauseMenuOpenCloseAction = _playerInput.actions["PauseMenuOpenClose"];
    }

    private void Update()
    {
        PauseMenuOpenCloseInput = _pauseMenuOpenCloseAction.WasPressedThisFrame();
    }
    #endregion
}
