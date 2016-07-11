using UnityEngine;
using System.Collections;

public class WormMouthSegment : WormBossSegment
{
    private bool patternActive = false;

    private bool randomMovement = true;

    private bool streamPattern = false;
    private bool sprayPattern = false;

    private int numProjs = 0;

    protected override void Movement()
    {
        
        if(streamPattern)
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

    public void MouthStreamPattern()
    {
        if (!patternActive)
        {
            StopAllCoroutines();
            randomMovement = false;
            patternActive = true;
            streamPattern = true;
            zSpeed = 1.0f;
            startZ = transform.eulerAngles.z;
            targetZ = Quaternion.LookRotation(Vector3.forward, (-(Player.instance.transform.position - transform.position))).eulerAngles.z;
            shootRate = 0.25f;

            numProjs = Random.Range(10, 31);

            targetLocation = true;
            zTime = 0.0f;
            StartCoroutine(MoveToTarget());
        }
    }

    public void MouthSprayPattern()
    {
        if (!patternActive)
        {
            StopAllCoroutines();
            randomMovement = false;
            patternActive = true;
            sprayPattern = true;
            zSpeed = 1.0f;
            shootRate = 0.75f;

            numProjs = Random.Range(25, 51);
            numProjs -= numProjs % 5;
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
                    break;
                }
                yield return null;
            }
        }
    }

    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (!boss.bossIsDefeated)
            {
                Movement();
                CoolDown();
                Shoot(projData, Vector3.down, shootTransform);
            }
        }
    }


    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
    {
        if (numProjs > 0)
        {
            _direction = Vector3.zero;
            if (streamPattern)
            {
                _direction = transform.rotation * Vector3.down;
                GameObject[] _shootTrans = new GameObject[1] { _shootTransform[Random.Range(0, _shootTransform.Length)] };
                if (base.Shoot(_projData, _direction, _shootTrans))
                {
                    --numProjs;
                    return true;
                }
            }
            else if(sprayPattern)
            {
                _direction = transform.rotation * Vector3.down;
                
                
                GameObject[] _shootTrans = new GameObject[1] { _shootTransform[3] };
                if (base.Shoot(_projData, _direction, _shootTrans))
                {
                    --numProjs;
                    for(int i = -2; i < 3; ++i)
                    {
                        if (i == 0) { continue; }

                        _direction = transform.rotation * (Vector3.down + (Vector3.right * 0.25f * i));
                        _shootTrans = new GameObject[1] { _shootTransform[i + 3] };
                        shootCooldown = shootRate;
                        base.Shoot(_projData, _direction, _shootTrans);
                        --numProjs;
                    }
                    
                    return true;
                }

            } 
        }
        else
        {
            if (patternActive)
            {
                StopAllCoroutines();
                streamPattern = false;
                sprayPattern = false;
                randomMovement = true;
                patternActive = false;
            }
        }
        return false;
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
        base.Reset();
    }
}
