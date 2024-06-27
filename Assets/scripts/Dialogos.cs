using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogos : MonoBehaviour
{
    // Asigna el prefab del objeto que quieres crear en el Inspector
    public GameObject objetoParaHacerVisible;

    // Desplazamiento en las coordenadas x y y
    public Vector2 desplazamiento;
    private Animator animator;
    private bool terminado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Trigger activado");

        if (other.tag == "Player" && terminado == false)
        {
            print("Haciendo visible el objeto encima del personaje");
            
            
            // Instancia el objeto con el desplazamiento especificado
            

            // Inicia la corrutina para hacer visible el objeto y luego ocultarlo
            StartCoroutine(HacerVisibleTemporalmente(other));
        }
    }
    
    // Corrutina para hacer visible el objeto temporalmente
    private IEnumerator HacerVisibleTemporalmente(Collider2D jugador)
    {
        terminado = true;
        PlayerMovement controladorJugador = jugador.GetComponent<PlayerMovement>();
        Animator animatorJugador = jugador.GetComponent<Animator>();
        if (controladorJugador != null)
        {
            controladorJugador.enabled = false;
        }
        yield return new WaitForSeconds(2f);

        
        // Mueve el objeto a la posición especificada con el desplazamiento
        Vector3 posicionDelJugador = jugador.transform.position;
        Vector3 posicionConDesplazamiento = new Vector3(posicionDelJugador.x + desplazamiento.x, posicionDelJugador.y + desplazamiento.y, posicionDelJugador.z);
        objetoParaHacerVisible.transform.position = posicionConDesplazamiento;
        objetoParaHacerVisible.SetActive(true);
        
        // Desactiva el control del jugador
        

        
        if (animatorJugador != null)
        {
            animatorJugador.enabled = false;
        }
        
        

        // Espera 4 segundos
        yield return new WaitForSeconds(13f);

        // Activa el control del jugador
        if (controladorJugador != null)
        {
            controladorJugador.enabled = true;
        }

        if (animatorJugador != null)
        {
            animatorJugador.enabled = true;
        }

        // Desactiva el objeto
        objetoParaHacerVisible.SetActive(false);
    }
}