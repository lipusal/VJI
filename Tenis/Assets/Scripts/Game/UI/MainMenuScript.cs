using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainScreen, settingsScreen;
    public Button[] setButtons;

//    private void Update()
//    {
//        foreach (var button in setButtons)
//        {
//            if (button.GetComponent<TextMeshProUGUI>().text == ScoreManager.GetInstance().MaxSets.ToString())
//            {
//                button.OnSelect(null);
//            }
//            else
//            {
//                button.OnDeselect(null);
//            }
//        }
//    }

    public void OnPlayClick(bool play2Players)
    {
        
        // Initialize some stuff
//        ScoreManager.SetGameMode(play2Players); implemented on master
        if (!play2Players)
        {
//            ScoreManager.SetDifficulty(1); implemented on master
        }
        SceneManager.LoadScene("TenisMatch");
    }
    
    public void OnSettingsClick()
    {
        GoToSettings();
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

    private void GoToSettings()
    {
        settingsScreen.SetActive(true);
        mainScreen.SetActive(false);
    }
    
    private void GoToMainScreen()
    {
        settingsScreen.SetActive(false);
        mainScreen.SetActive(true);
    }
}
