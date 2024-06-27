using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class FondoAjustado : MonoBehaviour
{
    public CinemachineVirtualCamera vcam; 
    public GameObject fondo; 
    public Transform personaje; 
    public float desfaseZBase = 0f; 
    public float desfaseYBase = 0f; 
    public float factorDesfaseZ = 0.1f; 
    public float factorDesfaseY = 10f;
    public int maxX;
    public int maxY;
    public int minX;
    public int minY;


    private SpriteRenderer fondoRenderer;
    private float initialFondoY; 

    void Start()
    {
        if (vcam == null)
        {
            Debug.LogError("Referencia a la cámara virtual no establecida.");
            return;
        }

        if (fondo == null)
        {
            Debug.LogError("Referencia al objeto de fondo no establecida.");
            return;
        }

        if (personaje == null)
        {
            Debug.LogError("Referencia al personaje no establecida.");
            return;
        }

        fondoRenderer = fondo.GetComponent<SpriteRenderer>();
        if (fondoRenderer == null)
        {
            Debug.LogError("El objeto de fondo no tiene un componente SpriteRenderer.");
        }

        initialFondoY = fondo.transform.position.y;
    }

    void Update()
    {
        AjustarFondo();
    }

    void AjustarFondo()
    {
        if (fondoRenderer == null) return;

        float orthoSize = vcam.m_Lens.OrthographicSize;

        float aspectRatio = Screen.width / (float)Screen.height;
        float cameraHeight = orthoSize * 2;
        float cameraWidth = cameraHeight * aspectRatio;
        cameraWidth *= 1.25f;
        cameraHeight *= 1.7f;

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Escenario5" || currentScene.name == "Escenario4")
        {
            cameraHeight *= 1.7f;
        }
        if (currentScene.name == "Escenario3")
        {
            cameraHeight *= 1.5f;
        }
        if (currentScene.name == "Escenario2")
        {
            cameraHeight *= 2f;
            cameraWidth *= 1.5f;
        }
   
        Vector2 spriteSize = fondoRenderer.sprite.bounds.size;
        Vector3 scale = fondo.transform.localScale;
        scale.x = cameraWidth / spriteSize.x;
        scale.y = cameraHeight / spriteSize.y;
        fondo.transform.localScale = scale;
        float desfaseZ = desfaseZBase + (orthoSize * factorDesfaseZ);
        Vector3 fondoPosition = personaje.position;
        fondoPosition.z += desfaseZ;

        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Escenario2")
        {
            float desfaseY = desfaseYBase + (orthoSize * factorDesfaseY);
            fondoPosition.y += desfaseY;
        }
        else
        {
            fondoPosition.y = initialFondoY; 
        }

        fondoPosition.x = Mathf.Clamp(fondoPosition.x, minX, maxX); 
        fondoPosition.y = Mathf.Clamp(fondoPosition.y, minY, maxY);

        fondo.transform.position = fondoPosition;
    }
}