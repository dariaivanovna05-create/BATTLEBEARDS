using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class VictoryController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer victoryRenderer;

    [SerializeField] private Sprite player1Sprite;
    [SerializeField] private Sprite player2Sprite;

    [SerializeField] private Sprite drawSprite;

    [SerializeField] private Button restartButton;
    [SerializeField] private string mainSceneName = "MainScene";
 
    void Start()
    {
        if(GameController.VictoryData.winner == PlayerIndex.Player1)
        {
            victoryRenderer.sprite = player1Sprite;
        }
        else if (GameController.VictoryData.winner == PlayerIndex.Player2)
        {
            victoryRenderer.sprite = player2Sprite;
        }
        else
        {
            victoryRenderer.sprite = drawSprite;
        }
 
        restartButton.onClick.AddListener(RestartGame);
    }
 
    private void RestartGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}
