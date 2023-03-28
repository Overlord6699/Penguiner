﻿using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get { return instance; } }
    private static AudioManager instance;

    [SerializeField] private float _musicVolume = 1;

    private AudioSource music1;
    private AudioSource music2;
    private AudioSource sfxSource;

    [SerializeField]
    private AudioClip _fishCollectSound;
    [SerializeField]
    private AudioClip _hitSound;

    private bool firstMusicSourceActive;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);

        music1 = gameObject.AddComponent<AudioSource>();
        music2 = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        music1.loop = true;
        music2.loop = true;
    }

    public void PlayFishCollectSFX()
    {
        PlaySFX(_fishCollectSound, 0.7f);
    }

    public void PlayHitSFX()
    {
        PlaySFX(_hitSound, 0.7f);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusicWithXFade(AudioClip musicClip, float transitionTime = 1.0f)
    {
        // Determine which source is active
        AudioSource activeSource = (firstMusicSourceActive) ? music1 : music2;
        AudioSource newSource = (firstMusicSourceActive) ? music2 : music1;

        firstMusicSourceActive = !firstMusicSourceActive;

        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithXFade(activeSource, newSource, musicClip, transitionTime));
    }
    private IEnumerator UpdateMusicWithXFade(AudioSource original, AudioSource newSource, AudioClip music, float transitionTime)
    {
        // Make sure the source is active and playing
        if (!original.isPlaying)
            original.Play();

        newSource.Stop();
        newSource.clip = music;
        newSource.Play();

        float t = 0.0f;

        for (t = 0.0f ; t <= transitionTime; t += Time.deltaTime)
        {
            original.volume = _musicVolume - ((t / transitionTime) * _musicVolume);
            newSource.volume = (t / transitionTime) * _musicVolume;
            yield return null;
        }

        original.volume = 0;
        newSource.volume = _musicVolume;

        original.Stop();
    }
}
