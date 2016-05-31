using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class soundManager : MonoBehaviour {

    public static soundManager instance;
    public int numberOfSources;
    public float volumeMultiplayer=1;
    List<AudioSource> audioSrcs = new List<AudioSource>();

    public AudioClip[] explosionSounds,hitSounds;

	void Awake ()
    {
        for (int i = 0; i < numberOfSources; i++)
        {
            audioSrcs.Add(gameObject.AddComponent<AudioSource>());
        }
        instance = this;
	}

    public void changeVolume()
    {
        //nothing atm
    }

    public void playSound(AudioClip sound)
    {
        int c = 0;
        while (c < audioSrcs.Count)
        {
            if (!audioSrcs[c].isPlaying)
            {
                audioSrcs[c].PlayOneShot(sound);
                break;
            }
            else
            {
                c++;
            }
        }
    }

    public void playSound(int type)//1 for explosions,0 for hit sounds.
    {
        int c = 0;
        while (c < audioSrcs.Count)
        {
            if (!audioSrcs[c].isPlaying)
            {
                switch (type)
                {
                    case 0:
                        audioSrcs[c].PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length - 1)]);
                        break;
                    case 1:
                        audioSrcs[c].PlayOneShot(explosionSounds[Random.Range(0,explosionSounds.Length-1)]);
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
