using UnityEngine;

public class LaserModule : MonoBehaviour
{
    [SerializeField] private LaserDamage damageModule = null;

    [SerializeField] private Animator animator = null;
    [SerializeField] private ParticleSystem chargeParticles = null;

    [SerializeField] private float laserSpeed = 25.0f;

    [SerializeField] private GameObject laserCore = null;
    [SerializeField] private GameObject laserTip = null;
    [SerializeField] private GameObject laserBack = null;

    private float laserTime = 0.0f;
    [SerializeField] private float chargeTime = 2.0f;
    [SerializeField] private float laserDuration = 2.0f;

    private bool fireLaser = false;
    private bool laserCharged = false;

    private const float SIZE = 13.0f / 16.0f;

    private Vector3 originalLocal;

    public void SetParameters(float _charge, float _duration, float _damage)
    {
        chargeTime = _charge;
        animator.SetFloat("Speed", 1.0f / chargeTime);
        laserDuration = _duration;
        damageModule.SetDamage(_damage);
    }

    private void Awake()
    {
        laserCore.SetActive(false);
        laserTip.SetActive(false);
        laserBack.SetActive(false);
        originalLocal = transform.localPosition;
        animator.SetFloat("Speed", 1.0f / chargeTime);
    }

    private void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (fireLaser)
            {
                laserTime += Time.deltaTime;
                if (laserCharged)
                {
                    if (laserTime < laserDuration)
                    {
                        if (CameraShake.instance.shakeAmount < 1)
                            CameraShake.instance.shakeAmount = 1;   
                        CameraShake.instance.shakeDuration += Time.deltaTime / 2f;

                        laserCore.transform.localScale = new Vector3(1,Mathf.Min(laserCore.transform.localScale.y + (Time.deltaTime * laserSpeed),50.0f),1);
                    }
                    else
                    {
                        laserCore.transform.localScale = new Vector3(1, Mathf.Max(laserCore.transform.localScale.y - (Time.deltaTime * laserSpeed / SIZE / 2.0f),0.0f), 1);
                        transform.localPosition = new Vector3(0.0f, transform.localPosition.y + ((Time.deltaTime * laserSpeed)/2.0f), 0.0f);
                        if(laserCore.transform.localScale.y == 0.0f)
                        {
                            fireLaser = false;
                            laserCore.SetActive(false);
                            laserTip.SetActive(false);
                            laserBack.SetActive(false);
                        }
                    }
                    laserTip.transform.localPosition = new Vector3(0.0f, (SIZE / 2.0f) * ((laserCore.transform.localScale.y * 2) + 1), 0.0f);
                }
                else
                {
                    //charging state
                    
                    if (CameraShake.instance.shakeAmount < laserTime/5 && CameraShake.instance.shakeDuration < 0.2f)
                        CameraShake.instance.shakeAmount = laserTime/5;
                    Debug.Log(laserTime / 100f);
                    CameraShake.instance.shakeDuration += Time.deltaTime/1.5f;

                    if (laserTime >= chargeTime)
                    {
                        laserTime = 0.0f;
                        laserCharged = true;
                        laserCore.SetActive(true);
                        laserTip.SetActive(true);
                        laserBack.SetActive(true);
                        animator.ResetTrigger("Charge");
                        soundManager.instance.playSound(4, laserDuration);
                        if (chargeParticles.isPlaying)
                        {
                            chargeParticles.Stop();
                        }
                    }
                }
            }
        }
    }

    public void  FireLaser()
    {
        if (!fireLaser)
        {
            fireLaser = true;
            laserCharged = false;
            laserTime = 0.0f;
            transform.localPosition = originalLocal;
            animator.SetTrigger("Charge");
            if (!chargeParticles.isPlaying)
            {
                chargeParticles.Play();
                soundManager.instance.playSound(3, chargeTime);
            }
        }
    }

    public void Reset()
    {
        laserCore.SetActive(false);
        laserTip.SetActive(false);
        laserBack.SetActive(false);
        
        fireLaser = false;
        laserCharged = false;
        laserTime = 0.0f;

        transform.localPosition = originalLocal;
        laserCore.transform.localScale = new Vector3(1, 0, 1);
        laserTip.transform.localPosition = new Vector3(0.0f, (SIZE / 2.0f) * ((laserCore.transform.localScale.y * 2) + 1), 0.0f);

        animator.ResetTrigger("Charge");
        if (chargeParticles.isPlaying)
        {
            chargeParticles.Stop();
        }
    }

    public bool IsLaserFiring()
    {
        return fireLaser;
    }
    public bool IsLaserCharging()
    {
        return !laserCharged;
    }



}
