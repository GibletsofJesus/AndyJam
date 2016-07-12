using UnityEngine;
using System.Collections;

public class WormLaserSegment : WormBossSegment
{
    [SerializeField] private LaserModule laser = null;

    private bool holdLaser = false;
    private Vector3 holdPosition;

    private bool sweepLaser = false;

    private bool repeatingLaser = false;
    private int repeating = 0;

    private bool laserFired = false;

    private bool patternActive = false;

    private bool randomMovement = true;

    protected override void Movement()
    {
        if (laserFired)
        {
            if (holdLaser)
            {
                if (!laser.IsLaserCharging())
                {
                    transform.eulerAngles = holdPosition;
                }
                else
                {
                    if (targetLocation)
                    {
                        targetZ = Quaternion.LookRotation(Vector3.forward, (-(Player.instance.transform.position - transform.position))).eulerAngles.z;
                    }
                    else 
                    {
                        startZ = transform.eulerAngles.z;
                        targetZ = Quaternion.LookRotation(Vector3.forward, (-(Player.instance.transform.position - transform.position))).eulerAngles.z;
                        targetLocation = true;
                        zTime = 0.0f;
                        StartCoroutine(MoveToTarget());
                    }
                }
            }
            if (!laser.IsLaserFiring())
            {
                if (repeatingLaser && repeating != 0)
                {
                    --repeating;
                    laser.SetParameters(1.0f, 0.1f, 2.0f);
                    laser.FireLaser();
                }
                else
                {
                    repeatingLaser = false;
                    patternActive = false;
                    laserFired = false;
                    randomMovement = true;
                    holdLaser = false;
                }
            }
        }
        if (randomMovement)
        {
            if (!targetLocation)
            {
                zSpeed = 1.0f;
                startZ = transform.eulerAngles.z;
                targetZ = Random.Range(minZ, maxZ);
                targetLocation = true;
                zTime = 0.0f;
                StartCoroutine(MoveToTarget());
            }
        }
    }

    public void LaserHoldPattern()
    {
        if (!patternActive)
        {
            StopAllCoroutines();
            randomMovement = false;
            patternActive = true;
            zSpeed = 1.0f;
            startZ = transform.eulerAngles.z;
            targetZ = Quaternion.LookRotation(Vector3.forward, (-(Player.instance.transform.position - transform.position))).eulerAngles.z;

            laser.SetParameters(5.0f, 3.0f, 1.5f);
            laser.FireLaser();
            laserFired = true;

            targetLocation = true;
            holdLaser = true;
            zTime = 0.0f;
            StartCoroutine(MoveToTarget());
        }
    }

    public void LaserSweepPattern()
    {
        if (!patternActive)
        {
            StopAllCoroutines();
            randomMovement = false;
            patternActive = true;
            zSpeed = 0.2f;
            startZ = transform.eulerAngles.z;
            targetZ = minZ;

            targetLocation = true;
            sweepLaser = true;
            zTime = 0.0f;
            
            laser.SetParameters(1.0f / zSpeed, 1.0f/ 0.5f, 1.5f);
            laser.FireLaser();
            laserFired = true;

            StartCoroutine(MoveToTarget());
        }
    }

    public void LaserRepeatingPattern()
    {
        if (!patternActive)
        {
            patternActive = true;
            zSpeed = 1.0f; ;

            repeatingLaser = true;

            laser.SetParameters(3.0f, 0.1f, 1.0f);
            laser.FireLaser();
            laserFired = true;
            repeating = Random.Range(3, 7);
        }
    }

    protected IEnumerator MoveToTarget()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            while (targetLocation)
            {
                zTime = Mathf.Min(zTime + (Time.deltaTime * zSpeed), 1.0f);
                float _lerpZ = Mathf.LerpAngle(startZ, targetZ, zTime);
                transform.eulerAngles = new Vector3(0.0f, 0.0f, _lerpZ);
                if (zTime == 1.0f)
                {
                    targetLocation = false;
                    if (holdLaser)
                    {
                        holdPosition = transform.eulerAngles;
                    }
                    if (sweepLaser)
                    {
                        targetLocation = true;
                        zTime = 0.0f;
                        zSpeed = 0.5f;
                        startZ = minZ;
                        targetZ = maxZ;
                        StartCoroutine(MoveToTarget());
                        sweepLaser = false;
                    }
                    break;
                }
                yield return null;
            }
        }
    }

    public override void Reset()
    {
        if (targetLocation)
        {
            StopAllCoroutines();
        }
        if (previousSegment)
        {
            previousSegment.Reset();
        }
        laser.Reset();
        base.Reset();
    }
}
