using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private Button restartButton;
    [SerializeField] private string mainSceneName = "MainScene";

    void Start()
    {
        if(GameController.VictoryData.winner == PlayerIndex.Player1)
        {
            victoryText.text = "Player 1 Wins!";
        }
        else
        {
            victoryText.text = "Player 2 Wins!";
        }

        restartButton.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }
    
}
