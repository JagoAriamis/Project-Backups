using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject CSharpMenuUI;
    public GameObject ControlsUI;
    public GameObject tipUI;
    public GameObject vectorsUI;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Resume();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        CSharpMenuUI.SetActive(false);
        ControlsUI.SetActive(true);
        tipUI.SetActive(true);
        vectorsUI.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        ControlsUI.SetActive(false);
        tipUI.SetActive(false);
        vectorsUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void ViewCSharpMenu()
    {
        pauseMenuUI.SetActive(false);
        CSharpMenuUI.SetActive(true);
    }

    public void ViewPseudocodeMenu()
    {
        pauseMenuUI.SetActive(true);
        CSharpMenuUI.SetActive(false);
    }
}

