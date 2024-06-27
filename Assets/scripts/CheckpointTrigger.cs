using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager checkpointManager = other.GetComponent<CheckpointManager>();
            if (checkpointManager != null)
            {
                checkpointManager.SetCheckpoint(transform.position,2);
                Debug.Log("Checkpoint activado en: " + transform.position);
            }
        }
    }
}