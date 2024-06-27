using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Importar el namespace UI para usar la clase Image

public class MenuPrincipal : MonoBehaviour
{
    public List<GameObject> objetosAMover; // Lista de objetos a mover
    public float desplazamientoX = 10f; // La distancia que los objetos se desplazarán en el eje X
    public float tiempoDesplazamiento = 2f; // El tiempo que tomará el desplazamiento
    public float oscilacionY = 0.5f; // La amplitud de la oscilación en el eje Y
    public float oscilacionFrecuencia = 2f; // La frecuencia de la oscilación en el eje Y

    public GameObject obj1; // El objeto específico a mover

    public GameObject ExplosionNave;
    public Vector3 pos1; // La primera posición objetivo para obj1
    public Vector3 pos2; // La segunda posición objetivo para obj1
    public float t1; // Tiempo que tomará el movimiento a pos1
    public float t2; // Tiempo que tomará el movimiento a pos2
    private Animator obj1Animator; // Referencia al Animator de obj1
    private bool termina;
    public Image fadeImage; // Referencia a la imagen para el fade in
    public float fadeDuration = 1f; // Duración del fade in
    List<Vector3> posicionesIniciales = new List<Vector3>();

    void Start()
    {
        termina = false;
        obj1Animator = obj1.GetComponent<Animator>(); // Obtener el Animator de obj1
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false); // Asegurarse de que la imagen esté desactivada al inicio
        }
    }

    public void Jugar()
    {
        Debug.Log("Jugar presionado, comenzando desplazamiento...");
        if (obj1Animator != null)
        {
            obj1Animator.SetBool("va", true); // Establecer el booleano 'va' en true
        }
        StartCoroutine(IniciarJuego());
    }

    public void Salir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

    private IEnumerator IniciarJuego()
    {
        foreach (GameObject objeto in objetosAMover)
        {
            posicionesIniciales.Add(objeto.transform.position);
            StartCoroutine(Oscillate(objeto)); // Iniciar la oscilación
        }
        if (objetosAMover.Count == 0)
        {
            Debug.LogWarning("No hay objetos en la lista para mover.");
            yield break;
        }

        float tiempoTranscurrido = 0;
        StartCoroutine(MoverObjeto(obj1, pos1, t1, pos2, t2)); // Iniciar el movimiento de obj1

        while (tiempoTranscurrido < tiempoDesplazamiento)
        {
            
            
            for (int i = 0; i < objetosAMover.Count; i++)
            {
                Vector3 posicionInicial = posicionesIniciales[i];
                Vector3 posicionObjetivo = new Vector3(posicionInicial.x + desplazamientoX, posicionInicial.y, posicionInicial.z);
                Vector3 nuevaPosicion = Vector3.Lerp(posicionInicial, posicionObjetivo, Mathf.SmoothStep(0, 1, tiempoTranscurrido / tiempoDesplazamiento));
                objetosAMover[i].transform.position = new Vector3(nuevaPosicion.x, objetosAMover[i].transform.position.y, nuevaPosicion.z); // Solo actualizar X
                Debug.Log($"Moviendo objeto {i}: {objetosAMover[i].name} a {objetosAMover[i].transform.position}");
            }
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
        for (int i = 0; i < objetosAMover.Count-1; i++)
        {
            objetosAMover[i].SetActive(false);
        }
        // Asegurar que los objetos lleguen a su posición final
        for (int i = 0; i < objetosAMover.Count; i++)
        {
            Vector3 posicionInicial = posicionesIniciales[i];
            Vector3 posicionObjetivo = new Vector3(posicionInicial.x + desplazamientoX, posicionInicial.y, posicionInicial.z);
            objetosAMover[i].transform.position = new Vector3(posicionObjetivo.x, objetosAMover[i].transform.position.y, posicionObjetivo.z); // Solo actualizar X
        }
        
        Debug.Log("Desplazamiento completo, comenzando fade in...");
        // Iniciar el fade in
        
        StartCoroutine(FadeInAndLoadScene());
    }

    private IEnumerator FadeInAndLoadScene()
    {
        fadeImage.gameObject.SetActive(true);
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color color = fadeImage.color;
            color.a = 0;
            fadeImage.color = color;

            float elapsedTime = 0;
            while (elapsedTime < fadeDuration)
            {
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                fadeImage.color = color;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Asegurarse de que la imagen esté completamente visible
            color.a = 1;
            fadeImage.color = color;
        }
        
        Debug.Log("Fade in completo, cargando siguiente escena...");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator MoverObjeto(GameObject  objeto, Vector3 posicion1, float tiempo1, Vector3 posicion2, float tiempo2)
    {
        termina=true;
        // Mover el objeto a la primera posición (pos1)
        float tiempoTranscurrido = 0;
        Vector3 posicionInicial = objeto.transform.position;

        while (tiempoTranscurrido < tiempo1)
        {
            objeto.transform.position = Vector3.Lerp(posicionInicial, posicion1, Mathf.SmoothStep(0, 1, tiempoTranscurrido / tiempo1));
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegurar que el objeto llegue a la primera posición
        objeto.transform.position = posicion1;

        // Esperar 2 segundos
        yield return new WaitForSeconds(2);

        // Mover el objeto a la segunda posición (pos2) y rotar 90 grados
        tiempoTranscurrido = 0;
        posicionInicial = objeto.transform.position;
        Quaternion rotacionInicial = objeto.transform.rotation;
        Quaternion rotacionFinal = rotacionInicial * Quaternion.Euler(0, 0, -85);

        while (tiempoTranscurrido < tiempo2)
        {
            objeto.transform.position = Vector3.Lerp(posicionInicial, posicion2, Mathf.SmoothStep(0, 1, tiempoTranscurrido / tiempo2));
            objeto.transform.rotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, Mathf.SmoothStep(0, 1, tiempoTranscurrido / tiempo2));
            tiempoTranscurrido += Time.deltaTime;
            
            yield return null;
        }

        // Asegurar que el objeto llegue a la segunda posición y complete la rotación
        objeto.transform.position = posicion2;
        objeto.transform.rotation = rotacionFinal;

        Debug.Log($"{objeto.name} ha llegado a {posicion2} y se ha girado 90 grados");
        yield return new WaitForSeconds(8);
        obj1Animator.SetBool("va", false);
        ExplosionNave.SetActive(true);

        
    }

    private IEnumerator Oscillate(GameObject objeto)
    {
        if (termina==false){
            Vector3 basePosition = objeto.transform.position;
            float elapsedTime = 0;

            while (elapsedTime < tiempoDesplazamiento)
            {
                if (termina==false){
                float yOffset = Mathf.Sin(elapsedTime * oscilacionFrecuencia) * oscilacionY;
                objeto.transform.position = new Vector3(objeto.transform.position.x, basePosition.y + yOffset, objeto.transform.position.z);
                elapsedTime += Time.deltaTime;
                yield return null;
                }
                else break;
            }

            // Asegurarse de que el objeto permanezca en su posición final
            objeto.transform.position = new Vector3(objeto.transform.position.x, basePosition.y, objeto.transform.position.z);
        }
    }
}
