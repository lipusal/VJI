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
            endGameText.text = playerWon ? "You Win" : "You Lose";
        }

        public void OnMainMenuClick()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
