using UnityEngine;
using System.Collections;

public class DecoracionHoja : MonoBehaviour
{
    public float fallDistance = 5f; // Distancia que el objeto debe moverse en el eje Y
    public float fallDuration = 3f; // Duración de la caída en segundos
    public float oscillationAmplitude = 0.5f; // Amplitud de la oscilación en el eje X
    public float oscillationFrequency = 1f; // Frecuencia de la oscilación en Hz
    public float probabilityCheckInterval = 1f; // Intervalo en segundos para comprobar la probabilidad
    public float waitAfterAnimation = 5f; // Tiempo en segundos a esperar después de una animación

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isFalling = false;
    private float elapsedTime = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No se encontró el componente SpriteRenderer.");
            return;
        }

        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(0, -fallDistance, 0);

        // Iniciar la corrutina para comprobar la probabilidad
        StartCoroutine(CheckProbability());
    }

    void Update()
    {
        if (isFalling)
        {
            FallWithOscillation();
        }
    }

    void FallWithOscillation()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / fallDuration;

        // Movimiento en Y
        float newY = Mathf.Lerp(startPosition.y, targetPosition.y, t);

        // Oscilación en X
        float newX = startPosition.x + Mathf.Sin(t * Mathf.PI * 2 * oscillationFrequency) * oscillationAmplitude;

        transform.position = new Vector3(newX, newY, transform.position.z);

        if (elapsedTime >= fallDuration)
        {
            isFalling = false;
            elapsedTime = 0f;
            transform.position = targetPosition;

            // Poner la opacidad en 0
            SetSpriteOpacity(0f);

            // Esperar un tiempo antes de restablecer la posición y opacidad
            StartCoroutine(ReactivateAfterDelay());
        }
    }

    public void StartFalling()
    {
        if (!isFalling)
        {
            isFalling = true;
            elapsedTime = 0f;
            startPosition = transform.position;
            targetPosition = startPosition + new Vector3(0, -fallDistance, 0);

            // Asegurarse de que la opacidad esté al 100%
            SetSpriteOpacity(1f);
        }
    }

    IEnumerator CheckProbability()
    {
        while (true)
        {
            if (!isFalling)
            {
                // Comprobar probabilidad
                if (Random.value < 0.3f) // Puedes ajustar la probabilidad aquí
                {
                    StartFalling();
                }
            }

            // Esperar el intervalo de comprobación
            yield return new WaitForSeconds(probabilityCheckInterval);

            // Si se está moviendo, esperar antes de volver a comprobar
            if (isFalling)
            {
                yield return new WaitForSeconds(waitAfterAnimation);
            }
        }
    }

    IEnumerator ReactivateAfterDelay()
    {
        yield return new WaitForSeconds(waitAfterAnimation);
        ResetPosition();
        SetSpriteOpacity(1f);
    }

    void ResetPosition()
    {
        transform.position = startPosition;
    }

    void SetSpriteOpacity(float opacity)
    {
        Color color = spriteRenderer.color;
        color.a = opacity;
        spriteRenderer.color = color;
    }
}
