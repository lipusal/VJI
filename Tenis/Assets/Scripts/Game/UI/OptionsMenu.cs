using System.Collections;
using System.Collections.Generic;
using Game.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
  private float _delayTime = 1f;
  public TextMeshProUGUI actionText;
  private int _option;

  public void Start()
  {
    bool playerWon = GameManagerLogic.GetPlayerWon();
    bool towPlayers = ScoreManager.GetInstance().IsTwoPlayers();
    _option = 0;
    if (towPlayers)
    {
      actionText.text = "MAIN MENU";
      _option = 1;
    }
    else if(playerWon)
    {
      actionText.text = "CONTINUE";
      _option = 2;
    }
    else
    {
      actionText.text = "RETRY";
      _option = 3;
    }
  }

  public void DoAction()
  {
    switch (_option)
    {
      case 1:
        GoToMainMenu();
        break;

      case 2:
        Continue();
        break;

      case 3:
        Retry();
        break;
    }
  }
  
  public void QuitGame()
  {
    Debug.LogFormat("Quit game");
    Application.Quit();
  }

  public void GoToMainMenu()
  {
    ScoreManager.GetInstance().ResetScore();
    Invoke(nameof(LoadMainMenu), _delayTime);
  }
  
  public void Continue()
  {
    ScoreManager scoreManager = ScoreManager.GetInstance();
    int difficulty = scoreManager.GetGameDifficulty();
    scoreManager.SetGameDifficulty(difficulty + 1);
    scoreManager.ResetScore();
    Invoke(nameof(LoadNextMatch), _delayTime);
  }

  public void Retry()
  {
    ScoreManager scoreManager = ScoreManager.GetInstance();
    int difficulty = scoreManager.GetGameDifficulty();
    scoreManager.SetGameDifficulty(difficulty);
    scoreManager.ResetScore();
    Invoke(nameof(LoadNextMatch), _delayTime);
  }
  
  private void LoadNextMatch()
  {
    SceneManager.LoadScene("TenisMatch");
  }
  private void LoadMainMenu()
  {
//    SceneManager.LoadScene("TenisMatch"); TODO replace with main menu
  }
}
