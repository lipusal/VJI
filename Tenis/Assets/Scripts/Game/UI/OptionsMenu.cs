using System.Collections;
using System.Collections.Generic;
using Game.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
  private float _delayTime = 0.5f;
  public TextMeshProUGUI actionText;
  public GameObject extraHardButton;
  private int _option;

  public void Start()
  {
    bool playerWon = GameManagerLogic.GetPlayerWon();
    bool twoPlayers = ScoreManager.GetInstance().IsTwoPlayers();
    int difficulty = ScoreManager.GetInstance().GetGameDifficulty();
    extraHardButton.SetActive(false);
    _option = 0;

    if (twoPlayers || (playerWon && difficulty >= 3))
    {
      if (difficulty == 3)
      {
        extraHardButton.SetActive(true);
      }
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

  public void ExtraHard()
  {
    ScoreManager scoreManager = ScoreManager.GetInstance();
    scoreManager.SetGameDifficulty(4);
    scoreManager.ResetScore();
    Invoke(nameof(LoadNextMatch), _delayTime);
  }
  
  private void LoadNextMatch()
  {
    SceneManager.LoadScene("TenisMatch");
  }
  private void LoadMainMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }
}
