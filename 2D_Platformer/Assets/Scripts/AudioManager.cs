 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager i;
    #region Static Instance
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned AudioManager",typeof(AudioManager)).GetComponent<AudioManager>();
                    DontDestroyOnLoad(instance);
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    #endregion

    #region Fields
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    private bool firstMusicSuorceIsPlaying;
    #endregion

    public bool isPlaying { get
        {
            return musicSource.isPlaying || musicSource2.isPlaying;
        }}

    private void Awake()
    {
        if (!i)
        {
            i = this;
            
        }
       
         
        

        musicSource = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();


        musicSource.loop = true;
        musicSource2.loop = true;
        SetMusicVolume(100);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activesource = (firstMusicSuorceIsPlaying) ? musicSource : musicSource2;


        activesource.clip = musicClip;
        activesource.volume = 1;
        activesource.Play();
    }
    
    public void PlayMusicWithFade(AudioClip newClip,float transitionTime=1.0f)
    {
        AudioSource activeSource = (firstMusicSuorceIsPlaying) ? musicSource : musicSource2;

        StartCoroutine(UptadeMusicWithFade(activeSource, newClip, transitionTime));

    }
    public void PlayMusicWithCrossFade(AudioClip musicClip,float transitionTime = 1.0f)
    {
        AudioSource activeSource = (firstMusicSuorceIsPlaying) ? musicSource : musicSource2;
        AudioSource newSource = (firstMusicSuorceIsPlaying) ? musicSource2 : musicSource;


        firstMusicSuorceIsPlaying = !firstMusicSuorceIsPlaying;

        newSource.clip = musicClip;
        newSource.Play();
        newSource.volume = 0;
        StartCoroutine(UptadeMusicWithCrossFade(activeSource, newSource, transitionTime));

    }

    private IEnumerator UptadeMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        float t= 0.0f;
        for (t = 0.0f; t <= transitionTime; t+= Time.deltaTime)
        {
            original.volume = (1 - (t / transitionTime));
            newSource.volume = (t / transitionTime);
            yield return null;
        }
        original.Stop();
    }
    private  IEnumerator UptadeMusicWithFade(AudioSource activeSource,AudioClip newClip,float transitionTime)
    {
        if (!activeSource.isPlaying)
            activeSource.Play();
        float t = 0.0f;

        for (t = 0; t < transitionTime; t+=Time.deltaTime)
        {
            activeSource.volume = (1 - (t / transitionTime));
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = ( t / transitionTime);
            yield return null;
        }
    }
    public void PlaySFX(AudioClip clip,float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicSource2.volume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public float GetSfxVolume()
    {
        return sfxSource.volume;
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }
}
