using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;
    private Vector3 initialPosition;

    void Awake()
    {
        // Guarda la posición inicial en el modo de ejecución
        if (Application.isPlaying)
        {
            initialPosition = transform.localPosition;
        }
    }

    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor;
        transform.localPosition = newPos;
    }

    public void ResetPosition()
    {
        if (Application.isPlaying)
        {
            transform.localPosition = initialPosition;
        }
    }
}
