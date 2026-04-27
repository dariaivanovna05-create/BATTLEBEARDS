using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static class VictoryData
    {
        public static PlayerIndex winner;
    }
    public static GameController instance { get; private set; }

    private PlayerActions actionsPlayer1 = PlayerActions.None;
    private PlayerActions actionsPlayer2 = PlayerActions.None;

    [Header("Music")]
    [SerializeField] AudioData MusicData;

    [Header("SFX")]
    [SerializeField] AudioData hornData;

    [Header("Data")]
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;
    [SerializeField] private float roundTime = 5f;
    [SerializeField] private float realizeTime = 1f;

    [Header("Countdown")]
    [SerializeField] private Animator countdownAnimator;
    [SerializeField] private string countdownStateName = "contadorAnim";

    [Header("Result Sprite")] // <-------------------------------- AQUI TIENES QUE METER PRIMERO UN SPRITE RENDERER PARA EL "LUGAR" DEL BOCADILLO Y, DESPUÉS, CADA BOCADILLO POR SEPARADO
    [SerializeField] private SpriteRenderer resultRenderer;
    [SerializeField] private Sprite spriteEmpate;
    [SerializeField] private Sprite spriteVictoriaP1;
    [SerializeField] private Sprite spriteVictoriaP2;

    [Header("Victory Scene")]
    [SerializeField] private string victorySceneName = "VictoryScene";

    private enum RoundResult { Draw, Player1Win, Player2Win }

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

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
        //resultRenderer.enabled = false;
        StartCoroutine(RoundLoop());
        
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(MusicData);
    }

    private IEnumerator RoundLoop()
    {
        while (true)
        {
            actionsPlayer1 = PlayerActions.None;
            actionsPlayer2 = PlayerActions.None;
            player1.RestartRound();
            player2.RestartRound();

            yield return new WaitForSeconds(roundTime);

            player1.LockInput(); // <── bloquear antes de revelar
            player2.LockInput();

            player1.RevealAction();
            player2.RevealAction();

            yield return StartCoroutine(ResolveRound());

            if (player1.IsDead() || player2.IsDead()) yield break;
        }
    }

    private IEnumerator ResolveRound()
    {
        RoundResult result = EvaluateResult(actionsPlayer1, actionsPlayer2);

        resultRenderer.enabled = true;

        switch (result)
        {
            case RoundResult.Draw:
                //resultRenderer.sprite = spriteEmpate; CAMBIAR ESTOOOOOOO
                break;

            case RoundResult.Player1Win:
                VictoryData.winner = PlayerIndex.Player1;
                //resultRenderer.sprite = spriteVictoriaP1; CAMBIAR ESTOOOOOOOOOO
                player2.Die();
                break;

            case RoundResult.Player2Win:
                VictoryData.winner = PlayerIndex.Player2;
                //resultRenderer.sprite = spriteVictoriaP2; CAMBIAR ESTOOOOOOOOO
                player1.Die();
                break;
        }

        yield return new WaitForSeconds(realizeTime);
        AudioManager.Instance.PlaySFX(hornData, transform.position);

        if (result == RoundResult.Player1Win || result == RoundResult.Player2Win)
        {
            SceneManager.LoadScene(victorySceneName);
        }
    }

    private void RegisterActions(PlayerIndex player, PlayerActions playerActions)
    {
        if (player == PlayerIndex.Player1) actionsPlayer1 = playerActions;
        else actionsPlayer2 = playerActions;
    }

    private static RoundResult EvaluateResult(PlayerActions action1, PlayerActions action2)
    {
        if (action1 == PlayerActions.Attack)
            if (action2 == PlayerActions.Reload || action2 == PlayerActions.None)
                return RoundResult.Player1Win;

        if (action2 == PlayerActions.Attack)
            if (action1 == PlayerActions.Reload || action1 == PlayerActions.None)
                return RoundResult.Player2Win;

        return RoundResult.Draw;
    }
}