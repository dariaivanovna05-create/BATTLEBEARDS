using UnityEngine;

public interface IAudioSystem
{
    void PlaySFX(AudioData data, Vector3 position = default);
    void PlayMusic(AudioData data);
    void StopMusic();
}

[CreateAssetMenu(menuName = "Audio/AudioData", fileName = "NewAudioData")]
public class AudioData : ScriptableObject
{
    [Header("Audio Clips")]
    public AudioClip [] clips;


    [Header("Settings")]
    [Range(0f,1f)] public float volume = 1f;
    [SerializeField] public Vector2 pitchRange = new Vector2(0.8f, 1.2f);
    [SerializeField] public bool isLooping = false;
    
    public AudioClip GetRandomAudioClip()
    {
        if(clips == null || clips.Length == 0)
        {
            Debug.LogWarning("No tienes Audio Clips dentro del ScriptableObject");
            return null;
        }
        return clips[Random.Range(0, clips.Length)];
    }

    public float GetRandomAudioPitch()
    {
        return Random.Range(pitchRange.x, pitchRange.y);
    }
}
