using UnityEngine;

public class Superponerobjeto : MonoBehaviour
{
    public GameObject objetoSuperpuesto; // El objeto que quieres superponer
    public Canvas canvas; // Tu Canvas

    void Start()
    {
        // Asegúrate de que el Canvas esté en World Space
        canvas.renderMode = RenderMode.WorldSpace;

        // Ajustar la posición del objeto para que se superponga
        Vector3 canvasPosition = canvas.transform.position;
        float offset = 0.1f; // Un pequeño desplazamiento para asegurarse de que esté en frente

        objetoSuperpuesto.transform.position = new Vector3(
            canvasPosition.x,
            canvasPosition.y,
            canvasPosition.z - offset
        );

        // Opcional: Escalar el objeto según el tamaño del Canvas
        objetoSuperpuesto.transform.localScale = canvas.transform.localScale;
    }
}