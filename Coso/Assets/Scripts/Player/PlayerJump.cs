using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour, IInterfaceJump
{
    private bool EnSuelo = false;
    public Rigidbody2D Rigidbody { get; private set; }
    private Transform DetectorSuelo;

    // Gestionan el intervalo de tiempo entre saltos
    private float CooldownJump = 0.5f;
    private float tiempoTranscurrido = 0f;


    [SerializeField]
    private LayerMask GroundLayer;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        DetectorSuelo = transform.Find("DetectorSuelo");

    }
    public void Jump(float jumpForce)
    {
        if (EnSuelo && tiempoTranscurrido >= CooldownJump)
        {
            Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            tiempoTranscurrido = 0f;
        }
    }
    public 

    void FixedUpdate()
    {
        Debug.Log(EnSuelo);
        EnSuelo = Physics2D.OverlapCircle(DetectorSuelo.position, 0.1f, GroundLayer);
        tiempoTranscurrido += Time.fixedDeltaTime;
    }

}