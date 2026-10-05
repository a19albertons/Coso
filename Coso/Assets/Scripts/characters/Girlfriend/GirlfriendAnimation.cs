using System;
using UnityEngine;
[RequireComponent(typeof(Animator))]

[RequireComponent(typeof(Rigidbody2D))]
public class GirlfriendAnimation : MonoBehaviour
{
    private static readonly int MovimientoEjeXHash = Animator.StringToHash("Movimiento Eje X");
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 framePrevio;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        framePrevio = new Vector2(transform.position.x, transform.position.y);
    }

    void Update()
    {
        ActualizarAnimaciones();
    }

    private void ActualizarAnimaciones()
    {
        Vector2 frameActual = new Vector2(transform.position.x, transform.position.y);
        float velocidadX = frameActual.x - framePrevio.x;
        animator.SetFloat(MovimientoEjeXHash, Math.Abs(velocidadX));
        framePrevio = frameActual;
    }
}