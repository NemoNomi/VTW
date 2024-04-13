using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneStartFadeIn : MonoBehaviour
{
    public Image fadeImage;        
    public float fadeDuration = 1f; 

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color initialColor = fadeImage.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }
}
