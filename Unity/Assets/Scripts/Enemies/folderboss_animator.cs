using UnityEngine;
using System.Collections;

public class folderboss_animator : MonoBehaviour {

    [SerializeField]
    private AudioClip RoarSound = null, GulpSound = null, GrowSound = null;

    public void shake()
    {
        CameraShake.instance.shakeDuration = 1;
        CameraShake.instance.shakeAmount = 1;
    }

    public void PlayBossMusic()
    {
        soundManager.instance.music.enabled = true;
    }

    public void Roar()
    {
        soundManager.instance.playSound(RoarSound);
    }

    public void Gulp()
    {
        soundManager.instance.playSound(GulpSound);
    }
    public void    Grow()
    {
        soundManager.instance.playSound(GrowSound);
    }

    public void End()
    {
        GetComponent<Animator>().enabled = false;
    }
}
