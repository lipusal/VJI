using System;
using System.Collections;
using System.Collections.Generic;
using Game.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public KeyCode[] pauseKeys = { KeyCode.Escape, KeyCode.JoystickButton7 }; // Start on PS, Menu on XBox (note, will poll any controller)
    public GameObject pauseMenu;
    public GameObject scoreboard;
    private bool _isPaused;

    private void Start()
    {
        _isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ActionMapper.IsButtonPressed(pauseKeys))
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
        scoreboard.SetActive(false);
        Time.timeScale = 0;
        _isPaused = true;
    }
    
    private void ResumeGame()
    {
        pauseMenu.SetActive(false);
        scoreboard.SetActive(true);
        Time.timeScale = 1;
        _isPaused = false;
    }
}
