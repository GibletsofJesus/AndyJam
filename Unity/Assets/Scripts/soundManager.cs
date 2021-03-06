﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class soundManager : MonoBehaviour
{

    public static soundManager instance;
    public int numberOfSources;
    public float volumeMultiplayer = 1,musicVolume;
    public AudioSource music;

    List<AudioSource> audioSrcs = new List<AudioSource>();

    public AudioClip[] explosionSounds, hitSounds;
    [SerializeField] private AudioClip winSound = null;

  public  AudioClip laserRelease= null;
    [SerializeField] private AudioClip laserCharge = null;
    [SerializeField] private AudioClip laserFire = null;
    [SerializeField] private AudioClip star = null;

    void Awake()
    {
        for (int i = 0; i < numberOfSources; i++)
        {
            audioSrcs.Add(gameObject.AddComponent<AudioSource>());
        }
        instance = this;
        changeVolume(.5f);
    }

    public void changeVolume(float newVol)
    {
        volumeMultiplayer = newVol;
    }
    public void changeMusicVolume(float newVol)
    {
        musicVolume = newVol;
        music.volume = musicVolume;
    }

    public void playSound(AudioClip sound, float pitch = 1, float volume = 1)
    {
        int c = 0;
        while (c < audioSrcs.Count)
        {
            if (!audioSrcs[c].isPlaying)
            {
                audioSrcs[c].pitch = pitch;
                audioSrcs[c].PlayOneShot(sound);
                audioSrcs[c].volume = volumeMultiplayer*volume;
                break;
            }
            if (audioSrcs[c].isPlaying && c == (audioSrcs.Count - 1))
            {
                audioSrcs.Add(gameObject.AddComponent<AudioSource>());
            }
            else
            {
                c++;
            }
        }
    }

    public void PauseAll()
    {
        foreach(AudioSource _s in audioSrcs)
        {
            _s.Pause();
        }
    }

    public void UnPauseAll()
    {
        foreach (AudioSource _s in audioSrcs)
        {
            _s.UnPause();
        }
    }

    public void playSound(int type, float pitchMod = 1.0f,float volume =1)//1 for explosions,0 for hit sounds.
    {
        int c = 0;
        while (c < audioSrcs.Count)
        {
            if (!audioSrcs[c].isPlaying)
            {
                switch (type)
                {
                    case 0:
                        audioSrcs[c].PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
                        audioSrcs[c].pitch = pitchMod;
                        audioSrcs[c].volume = volumeMultiplayer * .33f;
                        break;
                    case 1:
                        audioSrcs[c].PlayOneShot(explosionSounds[Random.Range(0,explosionSounds.Length-1)]);
                        audioSrcs[c].pitch = pitchMod;
                        audioSrcs[c].volume = volumeMultiplayer * .8f;
                        break;
                    case 2:
                        audioSrcs[c].PlayOneShot(winSound);
                        audioSrcs[c].pitch = pitchMod;
                        audioSrcs[c].volume = volumeMultiplayer * 2.0f;
                        break;
                    case 3:
                        audioSrcs[c].PlayOneShot(laserCharge);
                        audioSrcs[c].pitch = 1.0f / (pitchMod / 2.0f);
                        audioSrcs[c].volume = volumeMultiplayer * volume;
                        break;
                    case 4:
                        audioSrcs[c].PlayOneShot(laserFire);
                        audioSrcs[c].pitch = 1.0f / (pitchMod / 4.0f);
                        audioSrcs[c].volume = volumeMultiplayer * volume;
                        break;
                    case 5:
                        audioSrcs[c].PlayOneShot(star);
                        audioSrcs[c].pitch = pitchMod;
                        audioSrcs[c].volume = volumeMultiplayer * 2.0f;
                        break;
                }
                break;
            }
            else
            {
                c++;
            }
        }
    }
}
