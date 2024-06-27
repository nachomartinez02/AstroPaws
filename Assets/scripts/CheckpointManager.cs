using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    void Start()
    {
        // Inicialmente, la posición del último checkpoint es la posición inicial del jugador.
        lastCheckpointPosition = transform.position;
    }

    public void SetCheckpoint(Vector3 newCheckpointPosition, float yOffset)
    {
        // Sumar las unidades en el eje y a la nueva posición del checkpoint.
        newCheckpointPosition.y += yOffset;
        lastCheckpointPosition = newCheckpointPosition;
    }

    public Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}