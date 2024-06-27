using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class ZoomCamara : MonoBehaviour
{
    public CinemachineVirtualCamera vcam; 
    public float zoomOutFOV = 7f; 
    public float zoomSpeed = 2f; 
    public float zoomInFOV = 5f; 

    private Coroutine currentZoomCoroutine;
    void Start()
    {
        if (vcam == null)
        {
            var camera = Camera.main;
            var brain = (camera == null) ? null : camera.GetComponent<CinemachineBrain>();
            vcam = (brain == null) ? null : brain.ActiveVirtualCamera as CinemachineVirtualCamera;

            if (vcam == null)
            {
                Debug.LogError("No se encontró la cámara virtual de Cinemachine.");
                return;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.name); 
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger.");
            if (currentZoomCoroutine != null)
            {
                StopCoroutine(currentZoomCoroutine);
            }
            currentZoomCoroutine = StartCoroutine(ZoomOut());
        }
    }

    IEnumerator ZoomOut()
    {
        Debug.Log("ZoomOut coroutine started.");
        float startFOV = vcam.m_Lens.OrthographicSize;
        float endFOV = zoomOutFOV;
        float elapsed = 0f;

        while (elapsed < zoomSpeed)
        {
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(startFOV, endFOV, elapsed / zoomSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        vcam.m_Lens.OrthographicSize = endFOV;
        Debug.Log("ZoomOut coroutine ended."); 
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit called with: " + other.name);
        if (other.CompareTag("Player"))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            Debug.Log("Player exited the trigger."); 
            if (currentScene.name != "Escenario5")
            {
                if (currentZoomCoroutine != null)
                {
                    StopCoroutine(currentZoomCoroutine);
                }
                currentZoomCoroutine = StartCoroutine(ZoomIn());
            }
        }
    }

    IEnumerator ZoomIn()
    {
        Debug.Log("ZoomIn coroutine started."); 
        float startFOV = vcam.m_Lens.OrthographicSize;
        float endFOV = zoomInFOV;
        float elapsed = 0f;

        while (elapsed < zoomSpeed)
        {
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(startFOV, endFOV, elapsed / zoomSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        vcam.m_Lens.OrthographicSize = endFOV;
        Debug.Log("ZoomIn coroutine ended."); 
    }
}
