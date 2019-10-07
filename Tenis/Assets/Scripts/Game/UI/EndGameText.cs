using Game.GameManager;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class EndGameText : MonoBehaviour
    {
        public TextMeshProUGUI endGameText;
    
        // Start is called before the first frame update
        public void Start()
        {
            bool playerWon = GameManagerLogic.GetPlayerWon();
            endGameText.text = playerWon ? "You Win" : "You Lose";
        }
    }
}
