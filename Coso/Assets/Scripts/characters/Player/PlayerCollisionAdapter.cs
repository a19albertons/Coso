using UnityEngine;
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

    private void ColisionadorRampa()
    {
        boxCollider.enabled = false;
        circleCollider.enabled = true;
    }
    private void ColisionadorNormal()
    {
        boxCollider.enabled = true;
        circleCollider.enabled = false;
    }
}