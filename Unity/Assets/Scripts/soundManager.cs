using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class soundManager : MonoBehaviour
{

    public static soundManager instance;
    public int numberOfSources;
    public float volumeMultiplayer = 1;
    public AudioSource music;

    List<AudioSource> audioSrcs = new List<AudioSource>();

    public AudioClip[] explosionSounds, hitSounds;
    [SerializeField] private AudioClip winSound = null;

    [SerializeField] private AudioClip laserCharge = null;
    [SerializeField] private AudioClip laserFire = null;

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
        //nothing atm
    }

    public void playSound(AudioClip sound,float pitch = 1)
    {
        int c = 0;
        while (c < audioSrcs.Count)
        {
            if (!audioSrcs[c].isPlaying)
            {
                audioSrcs[c].pitch = pitch;
                audioSrcs[c].PlayOneShot(sound);
                audioSrcs[c].volume = volumeMultiplayer * .6f;
                break;
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

    public void playSound(int type, float pitchMod = 1.0f)//1 for explosions,0 for hit sounds.
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
                audioSrcs[c].volume = volumeMultiplayer * .4f;
                        break;
                    case 1:
                        audioSrcs[c].PlayOneShot(explosionSounds[Random.Range(0,explosionSounds.Length-1)]);
                audioSrcs[c].volume = volumeMultiplayer * .8f;
                        break;
                    case 2:
                        audioSrcs[c].PlayOneShot(winSound);
                        audioSrcs[c].volume = volumeMultiplayer * 2.0f;
                        break;
                    case 3:
                        audioSrcs[c].PlayOneShot(laserCharge);
                        audioSrcs[c].pitch = 1.0f / (pitchMod / 2.0f);
                        audioSrcs[c].volume = volumeMultiplayer * 2.5f;
                        break;
                    case 4:
                        audioSrcs[c].PlayOneShot(laserFire);
                        audioSrcs[c].pitch = 1.0f / (pitchMod / 4.0f);
                        audioSrcs[c].volume = volumeMultiplayer * 1.0f;
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
