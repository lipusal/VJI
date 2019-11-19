using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    public class GameManagerLogic : MonoBehaviour
    {
        private float delayTime = 3f;
        private static bool _playerWon;
    
        public void EndGame(bool playerWon)
        {
            ScoreManager scoreManager = ScoreManager.GetInstance();
            int difficulty = scoreManager.GetGameDifficulty();
            if (scoreManager.IsTwoPlayers() || !_playerWon || difficulty != 3)
            {
                _playerWon = playerWon;
                Invoke(nameof(LoadEndGameScene), delayTime);
            }
            else
            {
                scoreManager.SetGameDifficulty(difficulty + 1);
                scoreManager.ResetScore();
                Invoke(nameof(LoadNextMatch), delayTime);
            }
            
        }

        private void LoadEndGameScene()
        {
            SceneManager.LoadScene("GameEndedScene");
        }

        public static bool GetPlayerWon()
        {
            return _playerWon;
        }

        private void LoadNextMatch()
        {
            SceneManager.LoadScene("TenisMatch");
        }
    }
}
