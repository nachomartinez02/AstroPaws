using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para trabajar con UI

public class Entrada : MonoBehaviour
{
    
    public Animator otroObjetoAnimator; // Referencia al Animator del otro objeto
    public string siguienteEscena; // Nombre de la siguiente escena a cargar

    public Image fadeImage; // Referencia al componente Image del Canvas
    

    

    public FadeManager fadeManager;

    void Start()
    {
        
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }

        // Establecer la opacidad inicial
        

        // Iniciar el fade in
        StartCoroutine(FadeInImage());
    }

    private IEnumerator FadeInImage()
    {
        yield return StartCoroutine(fadeManager.FadeOut());
        
        
    }
}