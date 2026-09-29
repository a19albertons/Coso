using System;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerRump : MonoBehaviour, IInterfaceMovement
{
    // Elementos clave del jugador
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private new Rigidbody2D rigidbody2D;
    private RaycastHit2D hit;
    private RaycastHit2D hitIzquierda;
    private RaycastHit2D hitDerecha;
    private LayerMask floorLayer;
    public bool EnPendiente { get; private set; } = false;

    [SerializeField]
    private Vector2 maxVelocity = new(2.5f, 2.5f);

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        floorLayer = LayerMask.GetMask("Floor");
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.red);
        Debug.DrawRay(transform.position + new Vector3(-0.5f, 0f, 0f), Vector2.down * 0.52f, Color.blue);
        Debug.DrawRay(transform.position + new Vector3(0.5f, 0f, 0f), Vector2.down * 0.52f, Color.green);
    }

    void FixedUpdate()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, floorLayer);
        hitIzquierda = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0f, 0f), Vector2.down, 0.52f, floorLayer);
        hitDerecha = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0f, 0f), Vector2.down, 0.52f, floorLayer);

        // Detecta si estamos en una pendiente a partir de los raycasters
        if ((hit.normal.y != 1.00 && hit.normal.y != 0)
        || (hitIzquierda.normal.y != 1.00 && hitIzquierda.normal.y != 0)
        || (hitDerecha.normal.y != 1.00 && hitDerecha.normal.y != 0)
        )
        {
            boxCollider.enabled = false;
            circleCollider.enabled = true;
            EnPendiente = true;
        }

        // Controla el ascenso al llegar a la cima de la pendiente
        if (hit.normal.y == 1f // El raycaster central detecta plano
        && (hitIzquierda.normal.y == 0f || (hitDerecha.normal.y == 0f)) // Una de los raycaster laterales esta en el aire
        && (hitIzquierda.normal.y >= 1 || (hitDerecha.normal.y == 1)) // El otro raycaster tambien toca en plano
        )
        {
            RestablecerColisionadores();
        }

        // Determina si hemos saltado de la pendiente y estamos en el aire o volvemos a superficie plana bajando de cima al suelo
        if ((hitIzquierda.normal.y == 0f && hitDerecha.normal.y == 0f)
        || (hitIzquierda.normal.y >= 1f && hitDerecha.normal.y == 1f)
        )
        {
            RestablecerColisionadores();
        }


        // Debug.Log("PlayerRump: hit normal 1: " + hit.normal.y);
        // Debug.Log("PlayerRump: hitIzquierda normal 2: " + hitIzquierda.normal.y);
        // Debug.Log("PlayerRump: hitDerecha normal 3: " + hitDerecha.normal.y);
        // Debug.Log("Primer if primera linea: " + (hit.normal.y != 1.00 && hit.normal.y != 0));
        // Debug.Log("Primer if segunda linea: " + (hitIzquierda.normal.y != 1.00 && hitIzquierda.normal.y != 0));
        // Debug.Log("Primer if tercera linea: " + (hitDerecha.normal.y != 1.00 && hitDerecha.normal.y != 0));
        // Debug.Log("PlayerRump: EnPendiente: " + EnPendiente);
    }

    void RestablecerColisionadores()
    {
        boxCollider.enabled = true;
        circleCollider.enabled = false;
        EnPendiente = false;
    }

    public void Move(Vector2 direction)
    {
        // Eje X
        if (rigidbody2D.linearVelocity.x + direction.x > maxVelocity.x)
        {
            rigidbody2D.linearVelocityX = maxVelocity.x;

        }
        else if (rigidbody2D.linearVelocityX + direction.x < -maxVelocity.x)
        {
            rigidbody2D.linearVelocityX = -maxVelocity.x;
        }
        else
        {
            rigidbody2D.AddForce(new Vector2(direction.x, 0f), ForceMode2D.Impulse);
        }


        // Eje Y
        if (rigidbody2D.linearVelocity.y + direction.y > maxVelocity.y)
        {
            rigidbody2D.linearVelocityY = maxVelocity.y;
        }
        else if (rigidbody2D.linearVelocityY + direction.y < -maxVelocity.y)
        {
            rigidbody2D.linearVelocityY = -maxVelocity.y;
        }
        else
        {
            rigidbody2D.AddForce(new Vector2(0f, direction.y), ForceMode2D.Impulse);
        }
    }

    public Vector2 NormalMedia()
    {
        Vector2 normalMedia = Vector2.zero;
        int count = 0;
        if (hit.collider != null)
        {
            normalMedia += hit.normal;
            count++;
        }
        if (hitIzquierda.collider != null)
        {
            normalMedia += hitIzquierda.normal;
            count++;
        }
        if (hitDerecha.collider != null)
        {
            normalMedia += hitDerecha.normal;
            count++;
        }
        if (count > 0)
        {
            normalMedia /= count;
            return normalMedia.normalized;
        }
        return Vector2.zero;
    }

    public Vector2 NormalCentral()
    {
        Vector2 centroPendiente = Vector2.zero;
        int count = 0;
        if (hit.collider != null)
        {
            centroPendiente += hit.normal;
            count++;
        }
        if (count > 0)
        {
            centroPendiente /= count;
            return centroPendiente;
        }
        return Vector2.zero;
    }
}
