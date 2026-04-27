using System;
using UnityEngine;

public class PlayMusicMainMenu : MonoBehaviour
{
    [SerializeField] public AudioData MusicData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayMusic(MusicData, 0);
    }
}
