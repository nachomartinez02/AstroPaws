using UnityEngine;
using System.Collections;

public class Asteroides5 : MonoBehaviour
{
    public float moveDistance = 5f; // Distancia que el objeto debe moverse en el eje Y
    public float animationDuration = 2f; // Duración de la animación en segundos
    public float probabilityCheckInterval = 1f; // Intervalo en segundos para comprobar la probabilidad
    public float waitAfterAnimation = 23f; // Tiempo en segundos a esperar después de una animación

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMovingToTarget = false;
    private bool isMovingBack = false;
    private float elapsedTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
        {
            Debug.LogError("No se encontró el componente Animator.");
            return;
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("No se encontró el componente SpriteRenderer.");
            return;
        }

        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(0, moveDistance, 0);

        // Iniciar la corrutina para comprobar la probabilidad
        StartCoroutine(CheckProbability());
    }

    void Update()
    {
        if (isMovingToTarget)
        {
            MoveToTarget();
        }
        else if (isMovingBack)
        {
            MoveBack();
        }
    }

    void MoveToTarget()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / animationDuration;
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        if (elapsedTime >= animationDuration)
        {
            isMovingToTarget = false;
            isMovingBack = true;
            elapsedTime = 0f;
            transform.position = targetPosition;
        }
    }

    void MoveBack()
    {
        SetSpriteOpacity(0f);
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / animationDuration;
        transform.position = Vector3.Lerp(targetPosition, startPosition, t);

        if (elapsedTime >= animationDuration)
        {
            isMovingBack = false;
            elapsedTime = 0f;
            transform.position = startPosition;
            
            StartCoroutine(ReactivateAfterDelay());
        }
    }

    public void StartMovingToTarget()
    {
        if (!isMovingToTarget && !isMovingBack)
        {
            isMovingToTarget = true;
            elapsedTime = 0f;
            startPosition = transform.position;
            targetPosition = startPosition + new Vector3(0, moveDistance, 0);
            
            SetSpriteOpacity(1f);
        }
    }

    IEnumerator CheckProbability()
    {
        while (true)
        {
            if (!isMovingToTarget && !isMovingBack)
            {
                // Comprobar probabilidad
                if (Random.value < 0.3f) // Puedes ajustar la probabilidad aquí
                {
                    StartMovingToTarget();
                }
            }

            // Esperar el intervalo de comprobación
            yield return new WaitForSeconds(probabilityCheckInterval);

            // Si se está moviendo, esperar 23 segundos antes de volver a comprobar
            if (isMovingToTarget || isMovingBack)
            {
                yield return new WaitForSeconds(waitAfterAnimation);
            }
        }
    }

    IEnumerator ReactivateAfterDelay()
    {
        yield return new WaitForSeconds(waitAfterAnimation);
        SetSpriteOpacity(1f);
    }

    void SetSpriteOpacity(float opacity)
    {
        Color color = spriteRenderer.color;
        color.a = opacity;
        spriteRenderer.color = color;
    }
}