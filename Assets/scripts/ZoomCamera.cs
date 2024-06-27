using UnityEngine;

public class Enemigovayvuelve : MonoBehaviour
{
    public float moveDistance = 5f; // Distancia que el objeto debe moverse en el eje X
    public float animationDuration = 2f; // Duración de la animación en segundos

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

        // Iniciar el movimiento hacia la posición objetivo
        StartMovingToTarget();
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
        transform.position = Vector3.Lerp(targetPosition, startPosition, t);

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
}