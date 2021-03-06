﻿using UnityEngine;
using System.Collections;

public class WormBossSegment : BossSegment
{
    [SerializeField] protected float minZ = 275.0f;
    [SerializeField] protected float maxZ = 315.0f;

    protected float startZ;
    protected float targetZ;
    protected float zTime = 0.0f;
    [SerializeField] protected float zSpeed = 2.0f;

    protected bool targetLocation = false;

    [SerializeField] public WormBossSegment previousSegment = null;

    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (!boss.bossIsDefeated)
            {
                Movement();
            }
        }
    }

    public override void TakeDamage(float _damage)
    {
        if(IsInvoking("revertColour"))
        {
            CancelInvoke("revertColour");
        }
        spriteRenderer.color = Color.red;
        Invoke("revertColour", .1f);
        boss.TakeDamageFromSegment(_damage);
    }

    protected override void Movement()
    {
        if (!targetLocation)
        {
            startZ = transform.localEulerAngles.z;
            targetZ = Random.Range(minZ, maxZ);
            targetLocation = true;
            zTime = 0.0f;
            StartCoroutine(MoveToLocalTarget());
        }
                
    }

    protected override void Death()
    {
        if (boss.bossIsDefeated)
        {
            base.Death();
        }
    }

    protected virtual IEnumerator MoveToLocalTarget()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            while (targetLocation)
            {
                zTime = Mathf.Min(zTime + (Time.deltaTime * zSpeed), 1.0f);
                float _lerpZ = Mathf.LerpAngle(startZ, targetZ, zTime);
                transform.localEulerAngles = new Vector3(0.0f, 0.0f, _lerpZ);
                if(zTime == 1.0f)
                {
                    targetLocation = false;
                    break;
                }
                yield return null;
            }
        }
    }

    public override void Reset()
    {
        if(targetLocation)
        {
            StopCoroutine(MoveToLocalTarget());
        }
        if(previousSegment)
        {
            previousSegment.Reset();
        }
        //base.Reset();
    }
}
