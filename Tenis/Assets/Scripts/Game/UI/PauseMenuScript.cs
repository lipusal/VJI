using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Escape;
    public GameObject pauseMenu;
    private bool _isPaused;

    private void Start()
    {
        _isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void OnResumeClick()
    {
        ResumeGame();
    }
    
    public void OnMainMenuClick()
    {
        Time.timeScale = 1; // Unfreeze time before going back to menu
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitClick()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        _isPaused = true;
    }
    
    private void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        _isPaused = false;
    }
}
