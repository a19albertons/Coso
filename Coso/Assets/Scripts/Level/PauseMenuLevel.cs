using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuLevel : MonoBehaviour
{  
    [Header("Pause Menu UI")]
    [SerializeField]
    private GameObject pauseMenuUI;
    
    [SerializeField]
    private Button buttonResume;

    [SerializeField]
    private Button buttonRestart;

    [SerializeField]
    private Button buttonLevel;

    [SerializeField]
    private Button buttonGame;

    [Header("Player Input")]
    [SerializeField]
    private InputActionAsset playerInput;

    [Header("Player Controller Script")]
    [SerializeField]
    private MonoBehaviour playerControllerScript;

    private InputAction pauseAction;
    private bool isPaused;

    void Awake()
    {
        pauseAction = playerInput.FindActionMap("Gameplay").FindAction("Pause");
        buttonResume.onClick.AddListener(Continuar);
        buttonRestart.onClick.AddListener(Restart);
        buttonLevel.onClick.AddListener(ExitLevel);
        buttonGame.onClick.AddListener(ExitGame);
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

    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;
        playerControllerScript.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Resume();
    }

    public void ExitLevel()
    {
        Time.timeScale = 1f;
        isPaused = false;
        playerControllerScript.enabled = true;
        // SceneManager.LoadScene("LevelSelection");
        Resume();
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