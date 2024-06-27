using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C1P1 : MonoBehaviour
{
   
    public GameObject objetoParaHacerVisible;
    public GameObject objetoParaHacerVisible2;
    public Vector2 desplazamiento;
    private Animator animator;
     private Animator animator2;
    private bool terminado = false;


    void Start()
    {
        
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

   
    private IEnumerator HacerVisibleTemporalmente(Collider2D jugador)
    {
        yield return new WaitForSeconds(2f);
        PlayerMovement controladorJugador = jugador.GetComponent<PlayerMovement>();
        Animator animatorJugador = jugador.GetComponent<Animator>();

        
        if (controladorJugador != null)
        {
            controladorJugador.enabled = false;
        }

        if (animator2 != null && animator2.runtimeAnimatorController != null)
        {
            
            animator2.SetBool("Va",true);
        }
       
        yield return new WaitForSeconds(6.7f);

       
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            
            animator.SetBool("Rompe",true);
        }

        
        Vector3 posicionConDesplazamiento = objetoParaHacerVisible.transform.position;
        
        objetoParaHacerVisible.transform.position = posicionConDesplazamiento;
        objetoParaHacerVisible.SetActive(true);

       
        if (animatorJugador != null)
        {
            animatorJugador.enabled = false;
        }


        yield return new WaitForSeconds(2f);

       
        if (controladorJugador != null)
        {
            controladorJugador.enabled = true;
        }

      
        if (animatorJugador != null)
        {
            animatorJugador.enabled = true;
        }
        animator.SetBool("Termina",true);
        terminado = true;

       
        
    }
}