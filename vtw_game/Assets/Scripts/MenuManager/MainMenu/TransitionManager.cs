using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public IEnumerator FadeToBlack(float fadeDuration, CanvasGroup transitionPanelCanvasGroup)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            transitionPanelCanvasGroup.alpha = Mathf.Clamp01(time / fadeDuration);
            yield return null;
        }
    }

    public IEnumerator FadeFromBlack(float fadeDuration, CanvasGroup transitionPanelCanvasGroup)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            transitionPanelCanvasGroup.alpha = Mathf.Clamp01(1 - (time / fadeDuration));
            yield return null;
        }
    }
}
