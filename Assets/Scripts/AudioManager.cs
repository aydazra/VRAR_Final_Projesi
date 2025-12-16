// ==================== AudioManager.cs ====================
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Clips")]
    public AudioClip sunClip;
    public AudioClip earthClip;
    public AudioClip moonClip;

    private AudioSource audioSource;
    public AudioSource sfxSource;

    private Dictionary<string, AudioClip> clipDictionary;

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
        }

        audioSource = gameObject.AddComponent<AudioSource>();

        // Dictionary oluştur
        clipDictionary = new Dictionary<string, AudioClip>
        {
            { "sun", sunClip },
            { "earth", earthClip },
            { "moon", moonClip }
        };
    }

    public void PlayInfo(string bodyType)
    {
        if (clipDictionary.ContainsKey(bodyType))
        {
            audioSource.clip = clipDictionary[bodyType];
            audioSource.Play();
        }
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        if (sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }

    // 👇 BUNLAR EKLENECEK
    public void PlayClip(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopAll()
    {
        if (audioSource != null)
            audioSource.Stop();

        if (sfxSource != null)
            sfxSource.Stop();
    }
}