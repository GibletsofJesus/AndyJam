using UnityEngine;
using System.Collections;

public class soundManager : MonoBehaviour {

    public static soundManager instance;

    public AudioSource audioSrc;

	void Awake ()
    {
        instance = this;
	}

    public void changeVolume()
    {

    }

    public void playSound(AudioClip sound)
    {
        audioSrc.PlayOneShot(sound);
    }
}
