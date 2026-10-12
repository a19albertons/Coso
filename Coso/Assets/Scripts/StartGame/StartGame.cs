using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button continueButton;

    void Start()
    {
        newGameButton.onClick.AddListener(NewGame);
        if (ValidarBotonContinue())
        {
            continueButton.onClick.AddListener(ContinueGame);
        }
        else
        {
            continueButton.interactable = false;
            TextMeshProUGUI text = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.gray;
        }

        // Soporte para mando / teclado al empezar el juego, seleccionando el botón de nuevo juego por defecto
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
    }

    /// <summary>
    /// Valida si tenemos el save del slot 1 para permitir clickar el boton continuar
    /// </summary>
    /// <returns>Retorna si el save existe o no</returns>
    private bool ValidarBotonContinue()
    {
        return File.Exists(Path.Combine(Application.persistentDataPath, "slotGuardado1"));
    }

    /// <summary>
    /// Metodo que actua sobre el boton nuevo juego
    /// </summary>
    private void NewGame()
    {
        SaveManager.GetIntance().NewGame();
        SceneManager.LoadScene("Intro");
    }

    /// <summary>
    /// Metodo que actua sobre el boton de continuar
    /// </summary>
    private void ContinueGame()
    {
        SaveManager.GetIntance().LoadGame(1);
        SceneManager.LoadScene("MundoBase");
    }
}