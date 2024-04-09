using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionMapManager : MonoBehaviour
{
    #region Singleton Pattern
    public static PlayerActionMapManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Serialized Fields
    [SerializeField] private PlayerInput playerInput1;
    [SerializeField] private PlayerInput playerInput2;
    #endregion

    #region Public Methods
    public void SwitchActionMapsToUI()
    {
        playerInput1.SwitchCurrentActionMap("UI");
        playerInput2.SwitchCurrentActionMap("UI");
    }

    public void SwitchActionMapsToGameplay()
    {
        playerInput1.SwitchCurrentActionMap("Player1");
        playerInput2.SwitchCurrentActionMap("Player2");
    }
    #endregion
}
