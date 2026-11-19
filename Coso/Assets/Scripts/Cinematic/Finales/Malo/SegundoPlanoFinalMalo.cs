using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase que gestiona las operaciones en segundo plano durante la cinemática del final malo, incluyendo la preparación de la escena "MundoBase" y el control de la finalización de la cinemática inicial.
/// </summary>
public class SegundoPlanoFinalMalo : MonoBehaviour
{
    /// <summary>
    /// Prepara la carga de la escena "MundoBase", mientrás se ejecuta la cinemática para ser llamada cuando se haya terminado el guardado y la cinemática inicial
    /// </summary>
    /// <returns></returns>
    public void CambiarACreditos()
    {
        GestionEscenas.GetInstance().CambiarEscena("Creditos");
    }
}