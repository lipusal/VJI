using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainScreen, settingsScreen;
    
    public void OnPlayClick(bool play2Players)
    {
        if (play2Players)
        {
            // Do nothing until implemented
            return;
        }
        // Initialize some stuff
        
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
