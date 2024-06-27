using UnityEngine;

public class ParallaxCamera : MonoBehaviour 
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;

    private float oldPosition;

    void Start()
    {
        if (Application.isPlaying)
        {
            oldPosition = transform.position.x;
        }
    }

    void OnEnable()
    {
        if (Application.isPlaying)
        {
            oldPosition = transform.position.x; // Reinicia la posición antigua al habilitar
        }
    }

    void LateUpdate()  
    {
        if (Application.isPlaying && transform.position.x != oldPosition)
        {
            if (onCameraTranslate != null)
            {
                float delta = oldPosition - transform.position.x;
                onCameraTranslate(delta);
            }

            oldPosition = transform.position.x;
        }
    }
}
