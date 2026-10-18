using UnityEngine;

/// <summary>
/// Clase base que gestiona el movimiento de la sierra en el juego, proporcionando una estructura para que las clases derivadas implementen su propio comportamiento de movimiento.
/// </summary>
public abstract class SawMovementBase : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Velocidad de movimiento de la sierra")]
    protected float velocidad = 3f;

    void FixedUpdate()
    {
        MoverSierra();
    }


    protected abstract void MoverSierra();
}
