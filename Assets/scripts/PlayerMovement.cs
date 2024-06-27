using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using static Unity.Mathematics.math;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public int multiplicadorVelocidad;
    public Lava lava; 
    public Animator mounstro;
    private Rigidbody2D rb;
    public CinemachineVirtualCamera vcam;
    
    public GameObject fondo; // Referencia al objeto de fondo
    private Vector3 escalaOriginalFondo; // Escala original del fondo
    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    private float velocidadDeMovimiento = 400f;
    private bool sePuedeMover = false;
    [Range(0, 0.3f)][SerializeField] private float suavizadoDeMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;
    
    private bool girando = false;
    public Vector2 input;
    private bool isRespawning = false;
    private float FOV;

    [Header("Salto")]
    [SerializeField] private float fuerzaDeSalto;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool enSuelo;
    private bool salto = false;

    [Header("SaltoRegulable")]
    [Range(0,1)] public float multiplicadorCancelarSalto;
    public float multiplicadorGravedad;
    private float escalaGravedad;
    private bool botonSaltoArriba = true;

    [Header("Animacion")]
    private Animator animator;

    private CheckpointManager checkpointManager;

    public FadeManager fadeManager;

    private bool puedeSaltar = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaGravedad = rb.gravityScale;
        animator = GetComponent<Animator>();
        checkpointManager = GetComponent<CheckpointManager>();

        if (checkpointManager == null)
        {
            Debug.LogError("No se encontró CheckpointManager en el jugador.");
        }

        
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 3) 
        {
            animator.SetBool("Principio", true);
            StartCoroutine(animacionPrincipio(animator));
        }
        else
        {
            sePuedeMover = true; // Permitir el movimiento si no estamos en la escena 1
        }

        // Guarda la escala original del fondo
        
    }

    private IEnumerator animacionPrincipio(Animator animator)
    {
        yield return new WaitForSeconds(6f);
        animator.SetBool("Principio", false);
        sePuedeMover = true;
    }

    private void Update()
    {
        FOV = vcam.m_Lens.OrthographicSize;
        velocidadDeMovimiento = FOV * multiplicadorVelocidad;
        
        // Aplica el movimiento horizontal
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        movimientoHorizontal = input.x * velocidadDeMovimiento;

        // Actualiza la animación
        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));

        // Gestiona el salto
        if (sePuedeMover)
        {
            if (Input.GetButtonDown("Jump") && puedeSaltar)
            {
                if (input.y >= 0)
                {
                    salto = true;
                    puedeSaltar = false; // Desactiva la posibilidad de saltar nuevamente hasta que se suelte la barra espaciadora
                }
                else
                {
                    DesactivarPlataformas();
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                BotonSuelto();
                puedeSaltar = true; // Reactiva la posibilidad de saltar
            }
        }
    }

    private void DesactivarPlataformas()
    {
        Collider2D[] objetos = Physics2D.OverlapBoxAll(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        foreach (Collider2D item in objetos)
        {
            PlatformEffector2D platformEffector2D = item.GetComponent<PlatformEffector2D>();
            if (platformEffector2D != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), item.GetComponent<Collider2D>(), true);
            }
        }
    }

    private void BotonSuelto()
    {
        if (rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - multiplicadorCancelarSalto), ForceMode2D.Impulse);
        }
        botonSaltoArriba = true;
        salto = false;
    }

    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        animator.SetBool("enSuelo", enSuelo);

        if (sePuedeMover)
        {
            Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);
            salto = false;

            if (rb.velocity.y < 0 && !enSuelo)
            {
                rb.gravityScale = escalaGravedad * multiplicadorGravedad;
            }
            else
            {
                rb.gravityScale = escalaGravedad;
            }

            // Actualiza la escala del fondo
            
        }
    }

    public void Mover(float mover, bool saltar)
    {
        if (sePuedeMover)
        {
            Vector3 velocidadObjetivo = new Vector2(mover, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);

            if ((mover > 0 && !mirandoDerecha && !girando) || (mover < 0 && mirandoDerecha && !girando))
            {
                StartCoroutine(GirarGradualmente());
            }

            if (enSuelo && saltar && botonSaltoArriba)
            {
                enSuelo = false;
                rb.AddForce(new Vector2(0f, fuerzaDeSalto), ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator GirarGradualmente()
    {
        girando = true;
        float duration = 0.5f; // Duración de la rotación en segundos
        float timeElapsed = 0f;

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);

        while (timeElapsed < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        mirandoDerecha = !mirandoDerecha;
        girando = false;
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("muerte") && !isRespawning)
        {
            Debug.Log("Colisión con trigger de muerte.");
            if (checkpointManager != null)
            {
                Transform originalTarget = vcam.Follow;
                    vcam.Follow = null;
                    vcam.LookAt = null;

                    isRespawning= true;
                // Iniciar el proceso de respawn con fade
                StartCoroutine(Respawn(originalTarget));
            }
        }
    }

    private IEnumerator Respawn(Transform originalTarget)
    {
        
        isRespawning = true;

        if (fadeManager != null )
        {
            yield return StartCoroutine(fadeManager.FadeIn());

            // Obtener la posición del último checkpoint
            Vector3 checkpointPosition = checkpointManager.GetLastCheckpointPosition();
            
            // Teletransportar al jugador al último checkpoint
            transform.position = checkpointPosition;

            // Teletransportar la lava a la misma posición del checkpoint
            if (mounstro != null)
            {
                mounstro.SetBool("Restart", true);
                mounstro.SetBool("Pie", false);
                mounstro.SetBool("Andar", false);
            }
            if (lava != null)
            {
                lava.Teleport(checkpointPosition + new Vector3(-15f, -2f, 0));
                // Iniciar el movimiento suave de la lava después de teletransportarla
                lava.StartMove(250f, 55); // Mueve la lava 5 unidades en el eje X durante 2 segundos
            }
            
            print("FADEEE");
            vcam.Follow = originalTarget;
            yield return StartCoroutine(fadeManager.FadeOut());
            Debug.Log("Jugador y lava movidos al último checkpoint: " + checkpointPosition);
        }
        else
        {
            // Si no hay FadeManager, solo reubicar al jugador
            Vector3 checkpointPosition = checkpointManager.GetLastCheckpointPosition();
            transform.position = checkpointPosition;

            if (mounstro != null)
            {
                mounstro.SetBool("Restart", true);
                mounstro.SetBool("Pie", false);
                mounstro.SetBool("Andar", false);
            }
            // Teletransportar la lava a la misma posición del checkpoint
            if (lava != null)
            {
                lava.Teleport(checkpointPosition + new Vector3(-25f, -2f, 0));
                // Iniciar el movimiento suave de la lava después de teletransportarla
                lava.StartMove(250f, 55); // Mueve la lava 5 unidades en el eje X durante 2 segundos
            }
            
            yield return StartCoroutine(fadeManager.FadeOut());
            Debug.Log("Jugador y lava movidos al último checkpoint: " + checkpointPosition);
        }

        // Restaurar el objetivo de la vcam
        
       

        // Opcionalmente, puedes agregar un pequeño retraso aquí si es necesario
        
         isRespawning = false;
    }
}