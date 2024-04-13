using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;           
    public float fadeDuration = 1f;
    public int nextSceneIndex = 2;  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    IEnumerator FadeAndLoadScene()
    {
        float elapsedTime = 0f;
        Color initialColor = fadeImage.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f); 

        fadeImage.gameObject.SetActive(true); 

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }
}
