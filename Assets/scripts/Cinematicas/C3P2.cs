using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class C3P2 : MonoBehaviour
{
    public float duracionFade = 2f; // Duración del fade in o fade out en segundos
    [Range(0f, 1f)] public float opacidadInicial = 1f; // Opacidad inicial de la imagen
    public Animator otroObjetoAnimator; // Referencia al Animator del otro objeto
    public string siguienteEscena; // Nombre de la siguiente escena a cargar

    public  Image canvasImage; // Referencia al componente Image del Canvas
    private float alphaObjetivo; // Valor objetivo del alpha
    private float tiempoInicio; // Tiempo de inicio del fade

    void Start()
    {
        
        

        // Establecer la opacidad inicial
        Color colorInicial = canvasImage.color;
        colorInicial.a = opacidadInicial;
        canvasImage.color = colorInicial;

        // Iniciar el fade out
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        tiempoInicio = Time.time;
        float alphaInicial = canvasImage.color.a; // Guardamos el valor inicial del alpha
        alphaObjetivo = 0f; // Valor objetivo para el fade out

        while (Time.time - tiempoInicio < duracionFade)
        {
            float progreso = (Time.time - tiempoInicio) / duracionFade;
            float nuevoAlpha = Mathf.Lerp(alphaInicial, alphaObjetivo, progreso);
            Color nuevoColor = canvasImage.color;
            nuevoColor.a = nuevoAlpha;
            canvasImage.color = nuevoColor;
            yield return null;
        }

        // Asegurarse de que la imagen esté completamente transparente al final del fade out
        Color colorFinal = canvasImage.color;
        colorFinal.a = alphaObjetivo;
        canvasImage.color = colorFinal;

        // Iniciar el fade in después de un breve retraso
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        if (otroObjetoAnimator != null)
        {
            otroObjetoAnimator.SetBool("va", true);
        }
        else
        {
            Debug.LogError("El Animator del otro objeto no está asignado.");
        }

        yield return new WaitForSeconds(36f);

        tiempoInicio = Time.time;
        float alphaInicial = canvasImage.color.a; // Guardamos el valor inicial del alpha
        alphaObjetivo = 1f; // Valor objetivo para el fade in

        while (Time.time - tiempoInicio < duracionFade)
        {
            float progreso = (Time.time - tiempoInicio) / duracionFade;
            float nuevoAlpha = Mathf.Lerp(alphaInicial, alphaObjetivo, progreso);
            Color nuevoColor = canvasImage.color;
            nuevoColor.a = nuevoAlpha;
            canvasImage.color = nuevoColor;
            yield return null;
        }

        // Asegurarse de que la imagen esté completamente visible al final del fade in
        Color colorFinal = canvasImage.color;
        colorFinal.a = alphaObjetivo;
        canvasImage.color = colorFinal;

        // Activar la animación "va" del Animator del otro objeto

        // Esperar un tiempo antes de cargar la siguiente escena
        SceneManager.LoadScene(siguienteEscena);
    }
}
