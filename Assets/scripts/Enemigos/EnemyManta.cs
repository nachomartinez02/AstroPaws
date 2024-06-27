using UnityEngine;
using System.Collections;

public class EnemyManta : MonoBehaviour
{
    public float moveDistance = 5f; // Distancia que el objeto debe moverse en el eje X
    public float animationDuration = 2f; // Duración de la animación en segundos
    public float probabilityCheckInterval = 1f; // Intervalo en segundos para comprobar la probabilidad
    public float waitAfterAnimation = 23f; // Tiempo en segundos a esperar después de una animación

    private Animator animator;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMovingToTarget = false;
    private bool isMovingBack = false;
    private float elapsedTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró el componente Animator.");
            return;
        }

        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(moveDistance, 0, 0);

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
            animator.SetBool("isMoving", false);
            transform.position = targetPosition;
        }
    }

    void MoveBack()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / animationDuration;
        transform.position = Vector3.Lerp(targetPosition, startPosition, 0);

        if (elapsedTime >= animationDuration)
        {
            isMovingBack = false;
            elapsedTime = 0f;
            animator.SetBool("isMoving", false);
            transform.position = startPosition;
        }
    }

    public void StartMovingToTarget()
    {
        if (!isMovingToTarget && !isMovingBack)
        {
            isMovingToTarget = true;
            elapsedTime = 0f;
            startPosition = transform.position;
            targetPosition = startPosition + new Vector3(moveDistance, 0, 0);
            animator.SetBool("isMoving", true);
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
}