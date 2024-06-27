using System.Collections;
using UnityEngine;

public class EnemyCalamar : MonoBehaviour
{
    private bool puedeAtacar = true;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EdgeCollider2D edgeCollider;

    public float desplazamientoX = 5f; // Distancia en X que se desplazará el enemigo
    public float tiempoDesplazamiento = 2f; // Tiempo en el que se realizará el desplazamiento
    private Vector3 initialPosition; // Posición inicial del enemigo

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        initialPosition = transform.position;
        edgeCollider.enabled = false;
        StartCoroutine(ComprobarAtaque());
    }

    // Corrutina que comprueba la probabilidad de ataque
    IEnumerator ComprobarAtaque()
    {
        while (true)
        {
            if (puedeAtacar)
            {
                int probabilidad = Random.Range(1, 3); // Genera un número aleatorio entre 1 y 4
                if (probabilidad == 1)
                {
                    puedeAtacar = false;
                    Atacar();
                }
            }
            yield return new WaitForSeconds(1f); // Espera 1 segundo antes de volver a comprobar
        }
    }

    // Método que se llama cuando el enemigo ataca
    void Atacar()
    {
        // Iniciar la corutina de ataque
        StartCoroutine(AtaqueRoutine());
    }

    IEnumerator AtaqueRoutine()
    {
        // Fase 1: Aparecer gradualmente
        edgeCollider.enabled = true;
        yield return StartCoroutine(ChangeOpacity(0f, 1f, 1f)); // Cambia opacidad de 0 a 1 en 1 segundo
        

        // Fase 2: Desplazarse en X
        animator.SetBool("Ataca", true);
        yield return StartCoroutine(Desplazarse(new Vector3(desplazamientoX, 0, 0), tiempoDesplazamiento));

        // Fase 3: Desaparecer gradualmente
        yield return StartCoroutine(ChangeOpacity(1f, 0f, 1f)); // Cambia opacidad de 1 a 0 en 1 segundo

        // Desactivar el EdgeCollider2D y teletransportar a la posición inicial mientras permanece invisible
        edgeCollider.enabled = false;
        transform.position = initialPosition;

        // Finalizar ataque sin reaparecer
        animator.SetBool("Ataca", false);
        puedeAtacar = true; // Permitir que el enemigo pueda atacar nuevamente
    }

    IEnumerator ChangeOpacity(float from, float to, float duration)
    {
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;
        
        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(from, to, elapsedTime / duration);
            spriteRenderer.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar que el valor final se aplique
        color.a = to;
        spriteRenderer.color = color;
    }

    IEnumerator Desplazarse(Vector3 desplazamiento, float duration)
    {
        Vector3 startingPosition = transform.position;
        Vector3 targetPosition = startingPosition + desplazamiento;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar que la posición final se aplique
        transform.position = targetPosition;
    }
}
