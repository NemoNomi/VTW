using UnityEngine;

public class CursorManager : MonoBehaviour
{
    #region Singleton
    private static CursorManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Cursor Management
    void Start()
    {
        SetCursorVisible(false);
    }

    void Update()
    {
        HideCursorIfVisible();
    }

    private void SetCursorVisible(bool isVisible)
    {
        Cursor.visible = isVisible;
    }

    private void HideCursorIfVisible()
    {
        if (Cursor.visible)
        {
            SetCursorVisible(false);
        }
    }
    #endregion
}
