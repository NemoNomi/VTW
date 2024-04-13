using UnityEngine;
using System.Collections;

public class InteractionControllerBegin : MonoBehaviour
{
    public GameObject instructionBubble;
    public float initialDelay = 0.5f;
    private bool hasShownOnce = false;
    private AudioSource audioSource;

    private void Start()
    {
        instructionBubble.SetActive(false);
                audioSource = GetComponent<AudioSource>();
    }

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
                audioSource.Play();
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

    private IEnumerator ShowInstructionAfterDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        instructionBubble.SetActive(true);
        audioSource.Play();
        hasShownOnce = true;
    }
}
