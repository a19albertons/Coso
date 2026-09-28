using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerJump))]
public class PlayerEffects : MonoBehaviour
{
    // Efectos de sonido
    [SerializeField]
    [Tooltip("Efecto de sonido al caminar sobre hierba")]
    private AudioClip walkGrass;

    private AudioSource audioSource;
    private new Rigidbody2D rigidbody;
    private PlayerJump playerJump;

    private bool isPlaying = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerJump = GetComponent<PlayerJump>();
        audioSource.clip = walkGrass;
    }

    void Update()
    {
        // Debug.Log("PlayerEffects: isPlaying = " + isPlaying);
        if (Mathf.Abs(rigidbody.linearVelocityX) > 0.1f && playerJump.EnSuelo)
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