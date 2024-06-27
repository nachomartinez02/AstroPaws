using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2P1 : MonoBehaviour
{
    // Asigna el prefab del objeto que quieres crear en el Inspector
    public GameObject objetoParaAnimar;

    // Asigna el EdgeCollider2D del objeto en el Inspector
    

    // Asigna el objeto del jugador en el Inspector
    public GameObject player;

    // Desplazamiento en las coordenadas x y y
    public Vector2 desplazamiento;
    private Animator animator;
    private bool terminado = false;

    // Almacena la posición original del objeto
    private Vector3 posicionOriginal;
    // Almacena los puntos originales del EdgeCollider2D
    private Vector2[] puntosOriginalesCollider;

   

    void Start()
    {
        // Obtener el componente Animator del objeto
        animator = objetoParaAnimar.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró el componente Animator en el objeto para hacer visible.");
            return;
        }

        
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" )
        {
            StartCoroutine(HacerVisibleTemporalmente(other));
        }
    }

    // Corrutina para hacer visible el objeto temporalmente
    private IEnumerator HacerVisibleTemporalmente(Collider2D jugador)
    {
        // Espera 2 segundos
        

        // Mueve el objeto a la posición especificada con el desplazamiento
        Vector3 posicionConDesplazamiento = posicionOriginal + (Vector3)desplazamiento;
        objetoParaAnimar.transform.position = posicionConDesplazamiento;
        objetoParaAnimar.SetActive(true);
        yield return new WaitForSeconds(2f);

        // Asegúrate de que el Animator esté activo antes de usarlo
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetBool("Pie", true);
        }

        

        // Desplaza el EdgeCollider2D en el eje y
        // yield return StartCoroutine(MoverEdgeColliderY(desplazamientoColliderY, duracionTransicionColliderY));
        yield return new WaitForSeconds(2f);

        // Cambia a la animación "Andar" y ajusta la posición en el eje x gradualmente
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetBool("Andar", true);
            
           
        }

        yield return new WaitForSeconds(33f);

        // Cambia de nuevo a la animación "Pie" y restaura la posición original en el eje y gradualmente
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            
            print("NOPIE");
            
            animator.SetBool("Pie", false);
            
            
        }

        terminado = true;

        // Desactiva el objeto después de la animación si es necesario
    }

    

    
}