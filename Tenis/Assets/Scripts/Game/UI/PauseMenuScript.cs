using System;
using System.Collections;
using System.Collections.Generic;
using FrameLord;
using Game.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Escape;
    public GameObject pauseMenu;
    public TextMeshProUGUI player1CurrentGamePoints;
    public TextMeshProUGUI player2CurrentGamePoints;
    public TextMeshProUGUI player1FullPoints;
    public TextMeshProUGUI player2FullPoints;
    private bool _isPaused;

    private void Start()
    {
        _isPaused = false;
        GameEventDispatcher.Instance.AddListener(PointEvent.NAME, Callback);
    }

    private void Callback(object sender, GameEvent e)
    {
        PointEvent pe = (PointEvent) e;
        player1CurrentGamePoints.text = pe.NewCurrentGamePoints[0];
        player2CurrentGamePoints.text = pe.NewCurrentGamePoints[1];
        player1FullPoints.text = pe.NewFullPoints[0];
        player2FullPoints.text = pe.NewFullPoints[1];
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
