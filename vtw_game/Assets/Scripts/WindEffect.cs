using System.Collections;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    public float minForce = 1f;
    public float maxForce = 5f;
    private float currentForce;
    public Vector2 windDirection = Vector2.left;
    public LayerMask playerLayer;
    public float changeInterval = 2f;
    public float fadeDuration = 1f; // Dauer des Fade-In und Fade-Out
    private AudioSource audioSource;
    public PauseMenuManager pauseMenuManager;
    private bool wasGamePaused = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the game object!");
        }
        audioSource.volume = 0; // Start with volume at 0
        StartCoroutine(ChangeWindForceRoutine());
    }

    IEnumerator ChangeWindForceRoutine()
    {
        while (true)
        {
            currentForce = Random.Range(minForce, maxForce);
            yield return new WaitForSeconds(changeInterval);
        }
    }

    private void Update()
    {
        if (pauseMenuManager != null && pauseMenuManager.IsGamePaused())
        {
            if (audioSource.isPlaying && audioSource.volume > 0)
            {
                StartCoroutine(FadeAudioVolume(audioSource, 0, fadeDuration));
                wasGamePaused = true;
            }
        }
        else if (wasGamePaused && !audioSource.isPlaying && audioSource.volume > 0)
        {
            audioSource.Play();
            StartCoroutine(FadeAudioVolume(audioSource, 1, fadeDuration));
            wasGamePaused = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            audioSource.Play();
            StartCoroutine(FadeAudioVolume(audioSource, 1, fadeDuration)); // Fade-In
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            StartCoroutine(FadeAudioVolume(audioSource, 0, fadeDuration)); // Fade-Out
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(windDirection * currentForce, ForceMode2D.Force);
            }
        }
    }

    IEnumerator FadeAudioVolume(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        source.volume = targetVolume;
        if (targetVolume == 0)
        {
            source.Stop();
        }
    }
}
