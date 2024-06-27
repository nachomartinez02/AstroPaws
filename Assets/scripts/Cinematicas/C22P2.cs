using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para trabajar con UI

public class C22P2 : MonoBehaviour
{

    public Salida fadeInSceneChanger;
    public GameObject objetoParaAnimar;
    public GameObject objetoParaAnimar2;
    public Vector2 desplazamiento;
    private Animator animator;
    private Animator animator2;
    private bool terminado = false;
    public string siguienteEscena;
    public Image fadeImage; // Referencia a la imagen del Canvas para el fade in

    void Start()
    {
        // Obtener el componente Animator del objeto
        animator = objetoParaAnimar.GetComponent<Animator>();
        animator2 = objetoParaAnimar2.GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("No se encontró el componente Animator en el objeto para hacer visible.");
            return;
        }

        // Inicialmente, la imagen del fade debe ser completamente transparente
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && terminado == false)
        {
            StartCoroutine(HacerVisibleTemporalmente(other));
        }
    }

    private IEnumerator HacerVisibleTemporalmente(Collider2D jugador)
    {
        PlayerMovement controladorJugador = jugador.GetComponent<PlayerMovement>();
        Animator animatorJugador = jugador.GetComponent<Animator>();

        // Desactiva el control del jugador
        if (controladorJugador != null)
        {
            controladorJugador.enabled = false;
        }

        yield return new WaitForSeconds(0f);
        
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator2.SetBool("Invisible", true);
        }

        // Espera 2 segundos
        yield return new WaitForSeconds(2f);

        // Mueve el objeto a la posición especificada con el desplazamiento
        Vector3 posicionConDesplazamiento = objetoParaAnimar.transform.position;
        objetoParaAnimar.transform.position = posicionConDesplazamiento;
        objetoParaAnimar.SetActive(true);

        // Activa el Animator del jugador
        animator.SetBool("CierraBoca", true);

        // Espera 4 segundos
        yield return new WaitForSeconds(5f);

        // Comienza el fade in
        
        yield return new WaitForSeconds(2f);


        siguienteEscena = "CinematicaAgua";
        terminado = true;
        print(siguienteEscena);
        fadeInSceneChanger.StartFadeInAndChangeScene(siguienteEscena);
    }

    
}