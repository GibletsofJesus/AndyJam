using UnityEngine;

public class LaserModule : MonoBehaviour
{
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

    private void Awake()
    {
        laserCore.SetActive(false);
        laserTip.SetActive(false);
        laserBack.SetActive(false);
        transform.localPosition = Vector3.zero;
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
                        laserCore.transform.localScale = new Vector3(1,Mathf.Min(laserCore.transform.localScale.y + (Time.deltaTime * laserSpeed),50.0f),1);
                    }
                    else
                    {
                        laserBack.SetActive(true);
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
                    if (laserTime >= chargeTime)
                    {
                        laserTime = 0.0f;
                        laserCharged = true;
                        laserCore.SetActive(true);
                        laserTip.SetActive(true);
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
            transform.localPosition = Vector3.zero;
        }
    }

    public void Reset()
    {
        laserCore.SetActive(false);
        laserTip.SetActive(false);
        laserBack.SetActive(false);
        transform.localPosition = Vector3.zero;
        fireLaser = false;
        laserCharged = false;
        laserTime = 0.0f;
    }

    public bool IsLaserFiring()
    {
        return fireLaser;
    }

   

}
