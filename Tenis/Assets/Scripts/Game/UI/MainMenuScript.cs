using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
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

    public void OnQuitClick()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
