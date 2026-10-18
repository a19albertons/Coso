using UnityEngine;

/// <summary>
/// Clase que gestiona si el jugador esta muerto o no en una escena para prevenir alguna race condition
/// </summary>
public class PlayerIsDeath : MonoBehaviour
{
    public bool isDead = false;
}