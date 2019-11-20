using Game.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class GameEndScript : MonoBehaviour
    {
        public TextMeshProUGUI endGameText;
    
        public void Start()
        {
            bool playerWon = GameManagerLogic.GetPlayerWon();

            if (ScoreManager.GetInstance().IsTwoPlayers())
            {
                endGameText.text = playerWon ? "Player 1 Wins!" : "Player 2 Wins!";

            }
            else
            {
                if (ScoreManager.GetInstance().GetGameDifficulty() == 4 && playerWon)
                {
                    endGameText.text =  "CONGRATULATIONS YOU WON!";
                    endGameText.fontSize = 100.0f;

                }
                else
                {
                    endGameText.text = playerWon ? "You Win!" : "You Lose";
                }
            }
        }

        public void OnMainMenuClick()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
