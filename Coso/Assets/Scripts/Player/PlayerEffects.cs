using UnityEngine;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(PlayerController))]
public class PlayerEffects : MonoBehaviour
{
    // Efectos de sonido
    [SerializeField]
    [Tooltip("Efecto de sonido al caminar sobre hierba")]
    private AudioClip walkGrass;

    private AudioSource audioSource;
    private new Rigidbody2D rigidbody;
    private PlayerController playerController;

    private bool isPlaying = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        audioSource.clip = walkGrass;
    }

    void Update()
    {
        if (Mathf.Abs(rigidbody.linearVelocityX) > 0.1f && playerController.playerState.estadoActual == PlayerState.PlayerStates.Ground)
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