using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public ParticleSystem[] particles;

    public void explode(float _size = 1)
    {
        foreach (ParticleSystem p in particles)
        {
			p.startSize = _size;
            p.Play();

            soundManager.instance.playSound(1);

            if (CameraShake.instance.shakeDuration < 0.2f)
            {
                CameraShake.instance.shakeDuration = 0.2f;
                CameraShake.instance.shakeAmount = 0.5f;
            }
        }
        Invoke("turnOff", 1f);
    }

    void turnOff()
    {
        gameObject.SetActive(false);
    }
}