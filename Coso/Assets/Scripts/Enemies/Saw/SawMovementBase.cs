using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
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
