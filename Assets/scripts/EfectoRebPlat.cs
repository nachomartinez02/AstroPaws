using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectoRebPlat : MonoBehaviour
{
    public float desplazamiento = 0.2f; // La cantidad de desplazamiento vertical de la plataforma
    public float velocidadDeRebote = 0.1f; // La velocidad a la que la plataforma regresa a su posición original
    private Vector3 posicionOriginal; // La posición original de la plataforma
    private bool isRebounding = false; // Bandera para verificar si la plataforma está en el proceso de rebote

    private void Start()
    {
        // Guardar la posición original de la plataforma
        posicionOriginal = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto que colisiona es el detector de pies del jugador
        if (other.CompareTag("PlayerFeet") && !isRebounding)
        {
            // Iniciar el efecto de rebote
            StartCoroutine(Rebote());
        }
    }

    private IEnumerator Rebote()
    {
        isRebounding = true; // Marcar el inicio del rebote
        Vector3 posicionInferior = new Vector3(posicionOriginal.x, posicionOriginal.y - desplazamiento, posicionOriginal.z);

        // Mover la plataforma hacia abajo
        while (Vector3.Distance(transform.localPosition, posicionInferior) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, posicionInferior, velocidadDeRebote);
            yield return null;
        }

        // Esperar un momento antes de devolver la plataforma a su posición original
        yield return new WaitForSeconds(0.1f);

        // Mover la plataforma de vuelta a su posición original
        while (Vector3.Distance(transform.localPosition, posicionOriginal) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, posicionOriginal, velocidadDeRebote);
            yield return null;
        }

        // Asegurarse de que la plataforma esté exactamente en la posición original al final
        transform.localPosition = posicionOriginal;
        isRebounding = false; // Marcar el final del rebote
    }
}
