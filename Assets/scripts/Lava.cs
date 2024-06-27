using System.Collections;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private Coroutine moveCoroutine;

    public void StartMove(float distance, float duration)
    {
        // Si hay una corrutina en ejecución, detenerla
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveInX(distance, duration));
    }

    private IEnumerator MoveInX(float distance, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(distance, 0, 0);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; // Asegurarse de que la posición final sea exacta
        moveCoroutine = null; // Resetear la referencia a la corrutina
    }

    public void Teleport(Vector3 newPosition)
    {
        // Si hay una corrutina en ejecución, detenerla
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        transform.position = newPosition;
    }
}