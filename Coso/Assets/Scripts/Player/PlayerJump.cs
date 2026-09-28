using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour, IInterfaceJump
{
    public bool EnSuelo { get; private set; } = false;
    private Rigidbody2D Rigidbody;
    private BoxCollider2D posicionDetectorSuelo;

    // Gestionan el intervalo de tiempo entre saltos
    private float CooldownJump = 0.5f;
    private float tiempoTranscurrido = 0f;


    [SerializeField]
    private LayerMask GroundLayer;
    [SerializeField]
    private GameObject detectorSuelo;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        posicionDetectorSuelo = detectorSuelo.GetComponent<BoxCollider2D>();

    }
    public void Jump(float jumpForce)
    {
        // Debug.Log(EnSuelo);
        if (EnSuelo && tiempoTranscurrido >= CooldownJump)
        {
            Rigidbody.linearVelocityY = 0f;
            Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            tiempoTranscurrido = 0f;
            
        }
    }


    void FixedUpdate()
    {
        EnSuelo = Physics2D.OverlapBox(posicionDetectorSuelo.bounds.center, posicionDetectorSuelo.bounds.size, 0f, GroundLayer);
        // Debug.Log("PlayerJump: EnSuelo = " + EnSuelo);
        tiempoTranscurrido += Time.fixedDeltaTime;
    }

}