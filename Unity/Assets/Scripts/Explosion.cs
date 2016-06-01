using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public ParticleSystem[] particles;
    public enum ExploSize { small, medium, large };
    public ExploSize size;

    public void explode()
    {
        foreach (ParticleSystem p in particles)
        {
            p.Play();

            soundManager.instance.playSound(1);

            switch (size)
            {
                case ExploSize.small:
                    if (CameraShake.instance.shakeDuration < 0.2f)
                    {
                        CameraShake.instance.shakeDuration = 0.2f;
                        CameraShake.instance.shakeAmount = 0.5f;
                    }
                    break;
                case ExploSize.medium:
                    if (CameraShake.instance.shakeDuration < 0.3f)
                    {
                        CameraShake.instance.shakeDuration = 0.3f;
                        CameraShake.instance.shakeAmount = .8f;
                    }
                    break;
                case ExploSize.large:
                    if (CameraShake.instance.shakeDuration < 0.4f)
                    {
                        CameraShake.instance.shakeDuration = 0.4f;
                        CameraShake.instance.shakeAmount = 1.2f;
                    }
                    break;
            }
        }
        Invoke("turnOff", 1.2f);
    }

    void turnOff()
    {
        gameObject.SetActive(false);
    }
}