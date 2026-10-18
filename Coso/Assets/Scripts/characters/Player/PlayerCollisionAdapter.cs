using UnityEngine;

/// <summary>
/// Determina el collider a usar en función de la información proporcionada por el GroundDetector.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(GroundDetector))]
public class PlayerCollisionAdapter : MonoBehaviour
{

    // Componentes
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private GroundDetector groundDetector;

    private bool usandoRampa;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        groundDetector = GetComponent<GroundDetector>();
    }
    void FixedUpdate()
    {
        bool isOnSlope = groundDetector.IsOnSlope;

        if (usandoRampa != isOnSlope)
        {
            usandoRampa = isOnSlope;
            if (usandoRampa)
            {
                ColisionadorRampa();
            }
            else
            {
                ColisionadorNormal();
            }
        }
    }

    /// <summary>
    /// Cambia el collider del jugador a uno más adecuado para las rampas, desactivando el box collider y activando el circle collider.
    /// </summary>
    private void ColisionadorRampa()
    {
        boxCollider.enabled = false;
        circleCollider.enabled = true;
    }

    /// <summary>
    /// Cambia el collider del jugador a uno más adecuado para el suelo normal, desactivando el circle collider y activando el box collider.
    /// </summary>
    private void ColisionadorNormal()
    {
        boxCollider.enabled = true;
        circleCollider.enabled = false;
    }
}