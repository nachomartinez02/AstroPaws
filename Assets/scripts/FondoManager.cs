using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FondoManager : MonoBehaviour
{
    public List<GameObject> fadeOutGroups; 
    public List<GameObject> fadeInObjects;
    public float fadeDuration = 2f;
    private bool terminado = false;
    
    
    public Lava lava; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        
        if (other.CompareTag("Player") && terminado ==false)
        {
            Debug.Log("Player entered the trigger");
            foreach (GameObject group in fadeOutGroups)
            {
                if (group != null)
                {
                    Debug.Log("Starting FadeOut for group: " + group.name);
                    StartCoroutine(FadeOutObjects(group));
                }
                else
                {
                    Debug.LogWarning("Un objeto en fadeOutGroups es null");
                }
            }
            foreach (GameObject obj in fadeInObjects)
            {

                
                
                if (obj != null)
                {
                     
                    SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                         
                        Debug.Log("Starting FadeIn for object: " + obj.name);
                        StartCoroutine(FadeInSprite(spriteRenderer));
                    }
                    else print("NULLLLNOOOOFadeINNNNNNNNNNN");
                }
                else
                {
                    
                    Debug.LogWarning("Un objeto en fadeInObjects es null");
                }
            }
        }
    }

    IEnumerator FadeOutObjects(GameObject agrupador)
    {
        terminado = true;
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "BocaMounstro")
        {
            yield return new WaitForSeconds(8f);
        }
        
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>(agrupador.GetComponentsInChildren<SpriteRenderer>());
        float elapsedTime = 0f;
        List<Color> originalColors = new List<Color>();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                originalColors.Add(spriteRenderer.color);
            }
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            for (int i = 0; i < spriteRenderers.Count; i++)
            {
                if (spriteRenderers[i] != null)
                {
                    Color color = originalColors[i];
                    color.a = alpha;
                    spriteRenderers[i].color = color;
                }
            }

            yield return null;
        }
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            if (spriteRenderers[i] != null)
            {
                Color color = originalColors[i];
                color.a = 0f;
                spriteRenderers[i].color = color;
            }
        }

        Debug.Log("FadeOut complete for group: " + agrupador.name);
    }
    IEnumerator FadeInSprite(SpriteRenderer spriteRenderer)
    {
        
        Color originalColor = spriteRenderer.color;
        float elapsedTime = 0f;
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Escenario5"  )
        {
            
            yield return new WaitForSeconds(3f);
        }
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        Debug.Log("FadeIn complete for object: " + spriteRenderer.gameObject.name);

        
         currentScene = SceneManager.GetActiveScene();
                if (currentScene.name == "Escenario5")
                {
                    print("Lava empieza");
                    lava.StartMove(250f, 55f);
                }
        
    }
}