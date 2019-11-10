using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void OnPlayClick()
    {
        SceneManager.LoadScene("TenisMatch");
    }

    public void OnQuitClick()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
