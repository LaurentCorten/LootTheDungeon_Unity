using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f;

    public static FadeManager instance;

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

    public IEnumerator FadeIn()
    {
        fadeCanvasGroup.alpha = 1;
        float t = 0;
        
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = 1 - (t/ fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0;
    }

    public IEnumerator FadeOut()
    {
        fadeCanvasGroup.alpha = 0;
        float t = 0;
        
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = t/ fadeDuration;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1;
    }
}
