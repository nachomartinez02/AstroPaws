using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pompa : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;

    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false; // Asegúrate de que el PolygonCollider2D esté inicialmente desactivado
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el objeto Player tenga la etiqueta "Player"
        {
            // Desactivar el movimiento del personaje
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            // Activar el PolygonCollider2D
            polygonCollider.enabled = true;

            // Establecer enSuelo a true y horizontal a 0 en el Animator
            Animator animator = other.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("enSuelo", true);
                animator.SetFloat("Horizontal", 0f);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }
    }
}