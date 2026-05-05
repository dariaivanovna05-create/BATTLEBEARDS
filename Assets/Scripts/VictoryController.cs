using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class VictoryController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer victoryRenderer;

    [SerializeField] private Sprite player1Sprite;
    [SerializeField] private Sprite player2Sprite;

    [SerializeField] private Sprite tieSprite;

    [SerializeField] private Button restartButton;
    [SerializeField] private string mainSceneName = "MainScene";
 
    void Start()
    {
        if (GameController.VictoryData.isTie)
        {
            victoryRenderer.sprite = tieSprite;
        }
        else if(GameController.VictoryData.winner == PlayerIndex.Player1)
        {
            victoryRenderer.sprite = player1Sprite;
        }
        else if (GameController.VictoryData.winner == PlayerIndex.Player2)
        {
            victoryRenderer.sprite = player2Sprite;
        }
 
        restartButton.onClick.AddListener(RestartGame);
    }
 
    private void RestartGame()
    {
        GameController.VictoryData.isTie = false;
        SceneManager.LoadScene(mainSceneName);
    }
}
