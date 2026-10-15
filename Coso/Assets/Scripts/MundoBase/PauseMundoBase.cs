using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMundoBase : MonoBehaviour
{
    [Header("Pause Menu UI")]
    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private Button buttonResume;

    [SerializeField]
    private Button buttonSave;

    [SerializeField]
    private Button buttonGame;

    [Header("Player Input")]
    [SerializeField]
    private InputActionAsset playerInput;

    [Header("Player Controller Script")]
    [SerializeField]
    private PlayerController playerControllerScript;

    [Header("Escena final")]
    [SerializeField]
    private string nombreEscena;

    private InputAction pauseAction;
    private bool isPaused;

    void Awake()
    {
        pauseAction = playerInput.FindActionMap("Gameplay").FindAction("Pause");
        buttonResume.onClick.AddListener(Continuar);
        buttonGame.onClick.AddListener(ExitGame);
        buttonSave.onClick.AddListener(SaveGame);
        EvaluarRequisitosGuardar();
    }

    void OnEnable()
    {
        pauseAction.Enable();
        pauseAction.performed += TogglePause;
    }

    void OnDisable()
    {
        pauseAction.performed -= TogglePause;
        pauseAction.Disable();
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Continuar()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        playerControllerScript.enabled = true;
        Resume();
    }

    /// <summary>
    /// Evalua si cumple los requisitos para guardar la partida, si se cumplen habilita el boton de guardar, si no se cumplen lo deshabilita
    /// </summary>
    private void EvaluarRequisitosGuardar()
    {
        if (SaveManager.GetIntance().currentSave != null)
        {
            if (!SaveManager.GetIntance().currentSave.checkpoint.level1_8IsDone)
            {
                if (SaveManager.GetIntance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)) != null)
                {
                    if (SaveManager.GetIntance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)).isLevelCompleted == EnumStateLevelYStars.LevelYStars.Provisional
                        || SaveManager.GetIntance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)).isLevelCompleted == EnumStateLevelYStars.LevelYStars.Obtenida)
                    {
                        // Si se cumplen los requisitos, habilita el botón de guardar
                        buttonSave.interactable = true;
                    }
                }
            }
        }
        // Actua como un valor por defecto deshabilita el boton de guardar si no se cumplen los requisitos
        buttonSave.interactable = false;
        buttonSave.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
    }

    public void SaveGame()
    {
        GuardadoCentralizadoMundoBase.ConsolidarProgreso();
        GuardadoCentralizadoMundoBase.GuardarProgreso();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        playerControllerScript.enabled = false;
        EventSystem.current.SetSelectedGameObject(buttonResume.gameObject);
    }

    void Resume()
    {
        if (!isPaused)
        {
            return;
        }
        isPaused = false;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        playerControllerScript.enabled = true;
    }
}