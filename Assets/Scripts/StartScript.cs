using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private string GameSceneName = "StartGameScene";

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }
}
