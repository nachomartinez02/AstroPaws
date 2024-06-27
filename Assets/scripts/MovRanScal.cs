using System.Collections;
using UnityEngine;

public class MovRanScal : MonoBehaviour
{
    public Vector3 scaleAmount = new Vector3(0.5f, 0.5f, 0.5f); // Cantidad de escala en cada dirección
    public float interval = 0.5f; // Intervalo de tiempo en segundos para calcular la probabilidad
    public float speed = 1.0f; // Velocidad de la escala

    private bool scalingUp = true; // Indica si el objeto se está escalando hacia arriba
    private Vector3 baseScale; // Escala base del objeto (para escalas acumulativas)
    private Vector3 targetScale; // Escala objetivo del objeto
    private float journeyLength; // Distancia total a recorrer
    private float startTime; // Tiempo en el que comenzó la escala
    private Coroutine scaleCoroutine; // Referencia a la corutina de escala

    void Start()
    {
        baseScale = transform.localScale;
        targetScale = baseScale;
        scaleCoroutine = StartCoroutine(Scale());
    }

    void Update()
    {
        if (journeyLength > 0)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.localScale = Vector3.Lerp(baseScale, targetScale, Mathf.SmoothStep(0, 1, fractionOfJourney));
        }
    }

    public void SetBaseScale(Vector3 newScale)
    {
        baseScale = newScale;
        targetScale = baseScale;
        journeyLength = 0; // Reset journey length to avoid unwanted scaling
    }

    public void StopScaling()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
            scaleCoroutine = null;
        }
    }

    IEnumerator Scale()
    {
        while (true)
        {
            yield return new WaitUntil(() => Vector3.Distance(transform.localScale, targetScale) < 0.01f);
            yield return new WaitForSeconds(interval);
            float chance = Random.Range(0f, 1f);

            if (chance <= 0.25f)
            {
                baseScale = transform.localScale;
                startTime = Time.time;

                if (scalingUp)
                {
                    targetScale = baseScale + scaleAmount;
                }
                else
                {
                    targetScale = baseScale - scaleAmount;
                }

                journeyLength = Vector3.Distance(baseScale, targetScale);
                scalingUp = !scalingUp;
            }
        }
    }
}
