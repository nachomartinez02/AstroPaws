using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C3P1 : MonoBehaviour
{
    // Asigna el prefab del objeto que quieres crear en el Inspector
    public GameObject objetoParaHacerVisible;
    public GameObject objetoParaHacerVisible2;

    // Desplazamiento en las coordenadas x y y
    public Vector2 desplazamiento;
    private Animator animator;
    private Animator animator2;
    private bool terminado = false;


    void Start()
    {
        // Obtener el componente Animator del objeto
        animator = objetoParaHacerVisible.GetComponent<Animator>();
        animator2 = objetoParaHacerVisible2.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró el componente Animator en el objeto para hacer visible.");
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && terminado == false)
        {
            StartCoroutine(HacerVisibleTemporalmente(other));
        }
    }

    // Corrutina para hacer visible el objeto temporalmente
    private IEnumerator HacerVisibleTemporalmente(Collider2D jugador)
    {
        
        PlayerMovement controladorJugador = jugador.GetComponent<PlayerMovement>();
        Animator animatorJugador = jugador.GetComponent<Animator>();

        // Desactiva el control del jugador
        if (controladorJugador != null)
        {
            controladorJugador.enabled = false;
        }

        if (animator2 != null && animator2.runtimeAnimatorController != null)
        {
            
            animator2.SetBool("Va",true);
        }

        yield return new WaitForSeconds(2f);
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            
            animator.SetBool("Va2",true);
        }
       
        
        // Espera 2 segundos
       

        // Asegúrate de que el Animator esté activo antes de usarlo
        

        // Mueve el objeto a la posición especificada con el desplazamiento
        Vector3 posicionConDesplazamiento = objetoParaHacerVisible.transform.position;
        
        objetoParaHacerVisible.transform.position = posicionConDesplazamiento;
        objetoParaHacerVisible.SetActive(true);

        // Desactiva el Animator del jugador
        if (animatorJugador != null)
        {
            animatorJugador.enabled = false;
        }

        // Espera 4 segundos
        yield return new WaitForSeconds(5f);

        // Activa el control del jugador
        if (controladorJugador != null)
        {
            controladorJugador.enabled = true;
        }

        // Activa el Animator del jugador
        if (animatorJugador != null)
        {
            animatorJugador.enabled = true;
        }
        animator2.SetBool("Termina",true);
        terminado = true;

        // Desactiva el objeto después de la animación
        
    }
}