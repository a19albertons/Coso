using UnityEngine;

/// <summary>
/// Gestiona las corrutinas en segundo plano que se ejecutan durante la cinemática del final bueno, como la carga de la escena "MundoBase"
/// </summary>
public class SegundoPlanoFinalBueno : MonoBehaviour
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