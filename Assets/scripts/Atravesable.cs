using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f; // Fuerza del salto
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detecta si el jugador colisiona con una plataforma
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            PlatformEffector2D effector = collision.collider.GetComponent<PlatformEffector2D>();
            if (effector != null)
            {
                // Permite atravesar la plataforma desde abajo
                if (rb.velocity.y > 0)
                {
                    Physics2D.IgnoreCollision(collision.collider, playerCollider, true);
                }
                else
                {
                    Physics2D.IgnoreCollision(collision.collider, playerCollider, false);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Restaura las colisiones una vez que el jugador deja la plataforma
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            Physics2D.IgnoreCollision(collision.collider, playerCollider, false);
        }
    }
}
