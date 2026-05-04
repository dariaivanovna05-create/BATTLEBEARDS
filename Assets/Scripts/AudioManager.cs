using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioSystem
{
    public static IAudioSystem Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;

    [Header("SFX Pool")]
    [SerializeField] private int initialPoolSize = 15;
    private List<AudioSource> sfxPool = new List<AudioSource>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Crear el AudioSource de música por código
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewSFXSource();
        }
    }

    private AudioSource CreateNewSFXSource()
    {
        GameObject gObject = new GameObject("SFXSource");
        gObject.transform.SetParent(transform);
        AudioSource aSource = gObject.AddComponent<AudioSource>();
        sfxPool.Add(aSource); // ← esto también faltaba
        return aSource;
    }

    public AudioSource GetAviableSFXSource()
    {
        foreach (var source in sfxPool)
        {
            if (!source.isPlaying) return source;
        }
        return CreateNewSFXSource();
    }

    public void PlaySFX(AudioData data, Vector3 position = default)
    {
        if (data == null) return;
        AudioSource aSource = GetAviableSFXSource();
        aSource.transform.position = position;
        aSource.clip = data.GetRandomAudioClip();
        aSource.volume = data.volume;
        aSource.pitch = data.GetRandomAudioPitch();

        aSource.Play();
    }

    public void PlayMusic(AudioData data, int level)
    {
        if (data == null)
        {
            StopMusic();
            return;
        }

        AudioClip mClip = data.clips[level];
        if (musicSource.clip == mClip && musicSource.isPlaying) return;

        musicSource.clip = mClip;
        musicSource.volume = data.volume;
        musicSource.loop = data.isLooping;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
