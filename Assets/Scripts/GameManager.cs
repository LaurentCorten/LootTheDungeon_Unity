using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour //? Static ???
{
    public event Action OnStartNewLevel;

    [SerializeField] GameObject resultContainer;

    public void StartNewLevel()
    {
        OnStartNewLevel?.Invoke();
        StartCoroutine(FadeManager.instance.FadeIn());
    }

    public void EndLevel(bool isPlayerAlive)
    {
        resultContainer.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = isPlayerAlive ? "Victory" : "Game Over";
        StartCoroutine(FadeManager.instance.FadeOut());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
