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
            _playerWon = playerWon;
            Invoke(nameof(LoadEndGameScene), delayTime);
        }

        private void LoadEndGameScene()
        {
            SceneManager.LoadScene("GameEndScene");
        }

        public static bool GetPlayerWon()
        {
            return _playerWon;
        }
    }
}
