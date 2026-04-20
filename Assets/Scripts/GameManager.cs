using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioData MusicData;
    [SerializeField] AudioData SFXData;

    void Start()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(MusicData);
    }
    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            AudioManager.Instance.PlaySFX(SFXData);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            AudioManager.Instance.StopMusic();
        }
    }
}
