using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class ScrollingBckg : MonoBehaviour
{
    public float scrollSpeed = 0.1f; // Velocidad del desplazamiento
    public RawImage Imagen;
    Rect rect;

    void Start()
    {
        rect = Imagen.uvRect;
    }

    void Update()
    {
        rect.x += Time.deltaTime * scrollSpeed;
        Imagen.uvRect = rect;
    }
}
