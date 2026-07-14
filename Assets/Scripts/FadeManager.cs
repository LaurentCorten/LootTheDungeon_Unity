using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField] GameObject buttonContainer;
    public CanvasGroup menuScreen;
    public CanvasGroup gameHUD;
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
        menuScreen.alpha = 1;
        gameHUD.alpha = 0;
        float t = 0;
        
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            menuScreen.alpha = 1 - (t/ fadeDuration);
            gameHUD.alpha = t / fadeDuration;
            yield return null;
        }
        menuScreen.alpha = 0;
        gameHUD.alpha = 1;
        buttonContainer.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        menuScreen.alpha = 0;
        gameHUD.alpha = 1;
        float t = 0;
        
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            menuScreen.alpha = t/ fadeDuration;
            gameHUD.alpha = 1 - (t / fadeDuration);
            yield return null;
        }
        menuScreen.alpha = 1;
        gameHUD.alpha = 0;
        buttonContainer.SetActive(true);
    }
}
