using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Salida : MonoBehaviour
{
    public Image fadeImage; // Asigna esta imagen desde el inspector
    public float fadeDuration = 1.0f;

    void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade image is not assigned.");
        }
    }

    public void StartFadeInAndChangeScene(string nextSceneName)
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeInAndChangeScene(nextSceneName));
        }
    }

    IEnumerator FadeInAndChangeScene(string nextSceneName)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        print("HOLAA"+ nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}