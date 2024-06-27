using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class C5P2 : MonoBehaviour
{
    public float duracionFade = 2f;     
    [Range(0f, 1f)] public float opacidadInicial = 0f; 
    public Animator otroObjetoAnimator;
    public Animator otroObjetoAnimator2; 
    public string siguienteEscena; 
    public GameObject  fadeTexto1;
    public GameObject  fadeTexto2;
    public GameObject Image1;
    public GameObject miObjeto;
    public GameObject miObjeto2;
    public GameObject Objeto; 
    public GameObject Reloj; 
    public GameObject Objeto2; 
    public GameObject ObjetoCamaraCentrada; 

    public RawImage Video;
    public  VideoPlayer videoPlayer;
    private Image[] images;
    private float alphaObjetivo; 
    private SpriteRenderer spriteRenderer;
    private float tiempoInicio; 
   
    private float alphaObjetivoFadeIn = 1f; 
    private float alphaObjetivoFadeOut = 0f; 

    public CinemachineVirtualCamera vcamExterna; 


    public float retrasoFadeOut = 8f; 

    void Start()
    {
        spriteRenderer = Objeto.GetComponent<SpriteRenderer>();

        
        if (spriteRenderer == null)
        {
            Debug.LogError("El objeto no tiene un componente SpriteRenderer para realizar el fade.");
            return;
        }

        // Establecer la opacidad inicial
        Color colorInicial = spriteRenderer.color;
        colorInicial.a = opacidadInicial;
        spriteRenderer.color = colorInicial;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StartFade(other));
  
            if (vcamExterna != null)
            {
                vcamExterna.Follow = ObjetoCamaraCentrada.transform;
            }
            else
            {
                Debug.LogError("La referencia a la cámara externa no está asignada en el Inspector.");
            }
        }
    }

    IEnumerator StartFade(Collider2D other)
    {
   
        yield return FadeOut();


        yield return FadeIn(other);
    }

    IEnumerator FadeOut()
    {
        if (otroObjetoAnimator != null)
        {
            otroObjetoAnimator.SetBool("Toca", true);
        }
        else
        {
            Debug.LogError("El Animator del otro objeto no está asignado.");
        }
       
            yield return new WaitForSeconds(4f);
            otroObjetoAnimator2.SetBool("Explota", true);
        

        
    }

    IEnumerator FadeIn(Collider2D jugador)
    {
        
        
        yield return new WaitForSeconds(4f);

        yield return new WaitForSeconds(5f);
        tiempoInicio = Time.time;
        float alphaInicial = spriteRenderer.color.a;
        alphaObjetivo = 1f;
        
        while (Time.time - tiempoInicio < duracionFade)
        {
            float progreso = (Time.time - tiempoInicio) / duracionFade;
            float nuevoAlpha = Mathf.Lerp(alphaInicial, alphaObjetivo, progreso);
            Color nuevoColor = spriteRenderer.color;
            nuevoColor.a = nuevoAlpha;
            spriteRenderer.color = nuevoColor;
            yield return null;
        }

        Color colorFinal = spriteRenderer.color;
        colorFinal.a = alphaObjetivo;
        spriteRenderer.color = colorFinal;
        images = Reloj.GetComponentsInChildren<Image>();
        tiempoInicio = Time.time;


        Dictionary<Image, float> alphasIniciales = new Dictionary<Image, float>();
        foreach (Image image in images)
        {
            alphasIniciales[image] = image.color.a;
        }
        
        miObjeto2.SetActive(true);

        while (Time.time - tiempoInicio < duracionFade)
        {
            float progreso = (Time.time - tiempoInicio) / duracionFade;
            foreach (Image image in images)
            {
                 alphaInicial = alphasIniciales[image];
                float nuevoAlpha = Mathf.Lerp(alphaInicial, alphaObjetivo, progreso);
                Color nuevoColor = image.color;
                nuevoColor.a = nuevoAlpha;
                image.color = nuevoColor;
            }
            if (videoPlayer != null && Video != null)
            {
                float nuevoAlpha = Mathf.Lerp(0, alphaObjetivoFadeIn, progreso);
                Color nuevoColor = Video.color;
                nuevoColor.a = nuevoAlpha;
                Video.color = nuevoColor;
            }

            if (Video != null)
            {
                float nuevoAlpha = Mathf.Lerp(0, alphaObjetivoFadeIn, progreso);
                Color nuevoColor = Video.color;
                nuevoColor.a = nuevoAlpha;
                Video.color = nuevoColor;
            }
            yield return null;

        }
        
        miObjeto.SetActive(true);



        yield return new WaitForSeconds(retrasoFadeOut);
        
        // Iniciar el fade out
        StartCoroutine(FadeOut2());
        Image1.SetActive(true);
        yield return new WaitForSeconds(20);
        miObjeto.SetActive(false);

        fadeTexto1.SetActive(true);
        yield return new WaitForSeconds(1f);
        fadeTexto2.SetActive(true);

        yield return new WaitForSeconds(5f);
        Application.Quit();
    }



    IEnumerator FadeOut2()
    {
        tiempoInicio = Time.time;
        Dictionary<Image, float> alphasIniciales = new Dictionary<Image, float>();
        foreach (Image image in images)
        {
            alphasIniciales[image] = image.color.a;
        }

        while (Time.time - tiempoInicio < duracionFade)
        {
            float progreso = (Time.time - tiempoInicio) / duracionFade;
            foreach (Image image in images)
            {
                float alphaInicial = alphasIniciales[image];
                float nuevoAlpha = Mathf.Lerp(alphaInicial, alphaObjetivoFadeOut, progreso);
                Color nuevoColor = image.color;
                nuevoColor.a = nuevoAlpha;
                image.color = nuevoColor;
            }
            yield return null;
        }

        foreach (Image image in images)
        {
            Color colorFinal = image.color;
            colorFinal.a = alphaObjetivoFadeOut;
            image.color = colorFinal;
        }

    
    }
}
