using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerIndex playerIndex;
    public event Action<PlayerIndex, PlayerActions> OnActionChosen;
    private bool reloading = false;
    private bool hasDecided = false;
    private SpriteRenderer spriteRenderer;
    private static readonly Color colorReload = Color.red;
    private static readonly Color colorDefend = Color.blue;
    private static readonly Color colorAttack = Color.yellow;
    private static readonly Color colorDefault = Color.white;

//atacar
    [SerializeField] private Animator animHacha1;
    [SerializeField] private Animator animHacha2;

// defenderse
    [SerializeField] private Animator animPlayer1;
    [SerializeField] private Animator animPlayer2;

// recargar
    [SerializeField] private Animator animRecarga1;
    [SerializeField] private Animator animRecarga2;

// respawn hacha
    [SerializeField] private Animator animRespawn1;
    [SerializeField] private Animator animRespawn2;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
 
    void Update()
    {
       
        if(hasDecided) return;
 
        if(playerIndex == PlayerIndex.Player1)
        {
            InputManagerPlayer1();
        }
        else
        {
            InputManagerPlayer2();
        }
    }

    public void RestartRound()
    {
        hasDecided = false;
        spriteRenderer.color = colorDefault;
    }

    private void ChooseAction(PlayerActions action)
    {
        if(hasDecided) return;
 
        switch (action)
        {
            case PlayerActions.Reload:
                spriteRenderer.color = colorReload;
                reloading = true;
                break;
            case PlayerActions.Defend:
                spriteRenderer.color = colorDefend;
                break;
            case PlayerActions.Attack:
                if (!reloading) 
                {
                    return;
                }
                animHacha1.SetBool("Attack", true);
                reloading = false;
                break;
        }
       
        hasDecided = true;
        OnActionChosen?.Invoke(playerIndex, action);
    }

    public void Die()
    {
        if (gameObject)
        {
            gameObject.SetActive(false);
        }
    }
    private void InputManagerPlayer1()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            ChooseAction(PlayerActions.Reload);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            ChooseAction(PlayerActions.Defend);
        }
        else if(Keyboard.current.dKey.wasPressedThisFrame)
        {
            ChooseAction(PlayerActions.Attack);
        }
    }
 
    private void InputManagerPlayer2()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            ChooseAction(PlayerActions.Reload);
        }
        else if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            ChooseAction(PlayerActions.Defend);
        }
        else if(Keyboard.current.jKey.wasPressedThisFrame)
        {
            ChooseAction(PlayerActions.Attack);
        }
    }
}
