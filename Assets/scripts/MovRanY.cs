using System.Collections;
using UnityEngine;

public class MovRanY : MonoBehaviour
{
    public float distance = 1.0f; // Distancia de oscilación en el eje Y
    public float interval = 0.5f; // Intervalo de tiempo en segundos para calcular la probabilidad
    public float speed = 1.0f; // Velocidad del movimiento oscilante

    private bool movingUp = true; // Indica si el objeto se está moviendo hacia arriba
    private Vector3 basePosition; // Posición base del objeto (para desplazamientos acumulativos)
    private Vector3 targetPosition; // Posición objetivo del objeto
    private float journeyLength; // Distancia total a recorrer
    private float startTime; // Tiempo en el que comenzó el movimiento
    private Coroutine oscillateCoroutine; // Referencia a la corutina de oscilación

    void Start()
    {
        basePosition = transform.position;
        targetPosition = basePosition;
        oscillateCoroutine = StartCoroutine(Oscillate());
    }

    void Update()
    {
        if (journeyLength > 0)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(basePosition, targetPosition, Mathf.SmoothStep(0, 1, fractionOfJourney));
        }
    }

    public void SetBasePosition(Vector3 newPosition)
    {
        basePosition = newPosition;
        targetPosition = basePosition;
        journeyLength = 0; // Reset journey length to avoid unwanted movement
    }

    public void StopOscillation()
    {
        if (oscillateCoroutine != null)
        {
            StopCoroutine(oscillateCoroutine);
            oscillateCoroutine = null;
        }
    }

    IEnumerator Oscillate()
    {
        while (true)
        {
            yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPosition) < 0.01f);
            yield return new WaitForSeconds(interval);
            float chance = Random.Range(0f, 1f);

            if (chance <= 0.25f)
            {
                basePosition = transform.position;
                startTime = Time.time;

                if (movingUp)
                {
                    targetPosition = basePosition + new Vector3(0, distance, 0);
                }
                else
                {
                    targetPosition = basePosition - new Vector3(0, distance, 0);
                }

                journeyLength = Vector3.Distance(basePosition, targetPosition);
                movingUp = !movingUp;
            }
        }
    }
}
