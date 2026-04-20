using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerIndex playerIndex;
    public event Action<PlayerIndex, PlayerActions> OnActionChosen;

    private bool hasDecided = false;
    private SpriteRenderer spriteRenderer;

    private bool isArmed = false;
    private bool isProtected = false;
    private bool isSafe = false;
    private bool isDead = false;

    private PlayerActions lastAction;

    [Header("None")]
    [SerializeField] private Sprite IdleBlue;
    [SerializeField] private Sprite IdleRed;
    [SerializeField] private Sprite ArmedBlue;
    [SerializeField] private Sprite ArmedRed;

    [Header("Dead")]
    [SerializeField] private Sprite DeadIdleBlue;
    [SerializeField] private Sprite DeadArmedBlue;
    [SerializeField] private Sprite DeadIdleRed;
    [SerializeField] private Sprite DeadArmedRed;

    [Header("Defense")]
    [SerializeField] private Sprite DefIdleBlue;
    [SerializeField] private Sprite DefArmedBlue;
    [SerializeField] private Sprite DefIdleRed;
    [SerializeField] private Sprite DefArmedRed;

    [Header("Safe")]
    [SerializeField] private Sprite SafeIdleBlue;
    [SerializeField] private Sprite SafeArmedBlue;
    [SerializeField] private Sprite SafeIdleRed;
    [SerializeField] private Sprite SafeArmedRed;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    void Update()
    {
        if (hasDecided || isDead) return;

        if (playerIndex == PlayerIndex.Player1)
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
        isProtected = false;
        isSafe = false;

        UpdateSprite();
    }

    private void ChooseAction(PlayerActions action)
    {
        if (hasDecided || isDead) return;

        lastAction = action;

        switch (action)
        {
            case PlayerActions.Reload:
                isArmed = true;
                break;

            case PlayerActions.Defend:
                isProtected = true;
                break;

            case PlayerActions.Attack:
                if (!isArmed)
                {
                    return;
                }

                isArmed = false;
                break;
        }

        UpdateSprite();

        hasDecided = true;
        OnActionChosen?.Invoke(playerIndex, action);
    }

    public void Die()
    {
        isDead = true;
        UpdateSprite();
    }

    public void MarkAsSafe()
    {
        isSafe = true;
        UpdateSprite();
    }

    public void ClearSafe()
    {
        isSafe = false;
        UpdateSprite();
    }

    public PlayerActions GetLastAction()
    {
        return lastAction;
    }

    public bool IsArmed()
    {
        return isArmed;
    }

    public bool IsProtected()
    {
        return isProtected;
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void UpdateSprite()
    {
        if (playerIndex == PlayerIndex.Player1)
        {
            if (isDead)
            {
                spriteRenderer.sprite = isArmed ? DeadArmedRed : DeadIdleRed;
            }
            else if (isSafe)
            {
                spriteRenderer.sprite = isArmed ? SafeArmedRed : SafeIdleRed;
            }
            else if (isProtected)
            {
                spriteRenderer.sprite = isArmed ? DefArmedRed : DefIdleRed;
            }
            else
            {
                spriteRenderer.sprite = isArmed ? ArmedRed : IdleRed;
            }
        }
        else
        {
            if (isDead)
            {
                spriteRenderer.sprite = isArmed ? DeadArmedBlue : DeadIdleBlue;
            }
            else if (isSafe)
            {
                spriteRenderer.sprite = isArmed ? SafeArmedBlue : SafeIdleBlue;
            }
            else if (isProtected)
            {
                spriteRenderer.sprite = isArmed ? DefArmedBlue : DefIdleBlue;
            }
            else
            {
                spriteRenderer.sprite = isArmed ? ArmedBlue : IdleBlue;
            }
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
        else if (Keyboard.current.dKey.wasPressedThisFrame)
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
        else if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            ChooseAction(PlayerActions.Attack);
        }
    }
}