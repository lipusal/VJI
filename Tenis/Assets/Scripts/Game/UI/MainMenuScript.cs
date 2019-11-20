using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainScreen, settingsScreen, infoScreen;
    public TextMeshProUGUI setsTitle, gamesPerSetTitle;

    private void Update()
    {
        setsTitle.SetText($"Sets to play: {ScoreManager.GetInstance().MaxSets}");
        gamesPerSetTitle.SetText($"Games per set: {ScoreManager.GetInstance().GamesPerSet}");
    }

    public void OnPlayClick(bool play2Players)
    {
        // Initialize some stuff
        ScoreManager scoreManager = ScoreManager.GetInstance();
        scoreManager.ResetScore();
        scoreManager.SetGameMode(play2Players);
        if (!play2Players)
        {
            scoreManager.SetGameDifficulty(1);
        }
        SceneManager.LoadScene("TenisMatch");
    }
    
    public void OnSettingsClick()
    {
        GoToSettings();
    }

    public void OnInfoClick()
    {
        GoToInfo();
    }
    
    public void OnBackClick()
    {
        GoToMainScreen();
    }

    public void OnQuitClick()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
    
    public void SetMaxSets(int maxSets)
    {
        ScoreManager.GetInstance().MaxSets = maxSets;
    }
    
    public void SetGamesPerSet(int gamesPerSet)
    {
        ScoreManager.GetInstance().GamesPerSet = gamesPerSet;
    }

    private void GoToSettings()
    {
        settingsScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    private void GoToInfo()
    {
        infoScreen.SetActive(true);
        mainScreen.SetActive(false);
    }
    
    private void GoToMainScreen()
    {
        settingsScreen.SetActive(false);
        infoScreen.SetActive(false);
        mainScreen.SetActive(true);
    }
}
