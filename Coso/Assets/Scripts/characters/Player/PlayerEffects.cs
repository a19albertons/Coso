using UnityEngine;

/// <summary>
/// Clase que gestiona los efectos del jugador como caminar sobre el suelo.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundDetector))]
public class PlayerEffects : MonoBehaviour
{
    // Efectos de sonido
    [SerializeField]
    [Tooltip("Efecto de sonido al caminar sobre hierba")]
    private AudioClip walkGrass;

    private AudioSource audioSource;
    private new Rigidbody2D rigidbody;
    private GroundDetector groundDetector;

    private bool isPlaying = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        groundDetector = GetComponent<GroundDetector>();
        audioSource.clip = walkGrass;
    }

    void Update()
    {
        if (Mathf.Abs(rigidbody.linearVelocityX) > 0.1f && groundDetector.EnTierra)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                audioSource.Play();
                audioSource.loop = true;
            }

        }
        else
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}