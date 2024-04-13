using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController1Canvas : MonoBehaviour
{
    #region Assign Canvas
    public GameObject instructionBubble;
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
        }
    }
    #endregion
}

