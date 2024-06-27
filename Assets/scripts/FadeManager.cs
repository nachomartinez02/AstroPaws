using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections;
public class FadeManager : MonoBehaviour
{
    public Image fadeImage;
    public TextMeshProUGUI  fadeTexto; 
    public float fadeDuration = 0.5f;
    public float fadeDuration2 = 1f;
    private bool faded = false;

    void Start()
    {
        if (fadeImage != null)
        {
            
        }
    }

    public IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Clamp01(1f - elapsedTime / fadeDuration);
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if(faded == false) 
        {
            color.a = 0f;
            fadeImage.color = color;
            
            
            float elapsedTimeText = 0f;
            Color textColor = fadeTexto.color;


            while (elapsedTimeText < fadeDuration2)
            {
                
                elapsedTimeText += Time.deltaTime;
                yield return null;
            
            }
            elapsedTimeText = 0f;
            while (elapsedTimeText < fadeDuration2)
            {
                textColor.a = Mathf.Clamp01(1f - elapsedTimeText / fadeDuration2);
                fadeTexto.color = textColor;
                elapsedTimeText += Time.deltaTime;
                yield return null;
                
            }

            textColor.a = 0f;
            fadeTexto.color = textColor;
            faded= true;
            fadeTexto.gameObject.SetActive(false);

        }

    }
    

}