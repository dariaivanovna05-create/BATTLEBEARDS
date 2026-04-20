using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static class VictoryData
    {
        public static PlayerIndex winner;
    }
    public static GameController instance {get; private set;}

    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;
    [SerializeField] private float roundTime = 5f;
    [SerializeField] private string victorySceneName = "VictoryScene";
    private PlayerActions actionsPlayer1 = PlayerActions.None;
    private PlayerActions actionsPlayer2 = PlayerActions.None;
 
    private float currentTime;
    private enum RoundResult { Draw, Player1Win, Player2Win }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        player1.OnActionChosen += RegisterActions;
        player2.OnActionChosen += RegisterActions;
    }

    void OnDestroy()
    {
        player1.OnActionChosen -= RegisterActions;
        player2.OnActionChosen -= RegisterActions;
    }

    void Start()
    {
        StartRound();
    }
    void Update()
    {
        currentTime -= Time.deltaTime;
        if(currentTime <= 0f)
        {
            ResolveRound();
        }
    }

    private void RegisterActions(PlayerIndex player, PlayerActions playerActions)
    {
        if(player == PlayerIndex.Player1)
        {
            actionsPlayer1 = playerActions;
        }
        else
        {
            actionsPlayer2 = playerActions;
        }
    }

    private void StartRound()
    {
        currentTime = roundTime;
        actionsPlayer1 = PlayerActions.None;
        actionsPlayer2 = PlayerActions.None;
 
        player1.RestartRound();
        player2.RestartRound();
    }

    private void ResolveRound()
    {
        switch (EvaluateResult(actionsPlayer1, actionsPlayer2))
        {
            case RoundResult.Draw:
               StartRound();
                break;
    
            case RoundResult.Player1Win:
                VictoryData.winner = PlayerIndex.Player1;
                player2.Die();
                SceneManager.LoadScene(victorySceneName);
                break;

            case RoundResult.Player2Win:
                VictoryData.winner = PlayerIndex.Player2;  
                player1.Die();
                SceneManager.LoadScene(victorySceneName);
                break;
        }
    }

    private static RoundResult EvaluateResult(PlayerActions action1, PlayerActions action2)
    {
        if(action1 == PlayerActions.Attack)
        {
            if(action2 == PlayerActions.Reload || action2 == PlayerActions.None)
            {
                return RoundResult.Player1Win;
            }
        }
 
        if(action2 == PlayerActions.Attack)
        {
            if(action1 == PlayerActions.Reload || action1 == PlayerActions.None)
            {
                return RoundResult.Player2Win;
            }
        }

        return RoundResult.Draw;
    }
}
