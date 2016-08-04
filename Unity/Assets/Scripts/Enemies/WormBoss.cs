using UnityEngine;
using System.Collections;

public class WormBoss : Boss
{
    [SerializeField] private WormLaserSegment laserSegment = null;
    [SerializeField] private WormMouthSegment mouthSegment = null;

    [SerializeField] private SpriteRenderer bossRenderer = null;
    // [SerializeField] private ParticleSystem spawnParticles;

    private bool atStartPosition = false;
    [SerializeField] private Vector3 startPosition = Vector3.zero;

    [SerializeField] private float laserPatternCooldown = 10.0f;
    private float laserTime = 0.0f;
    [SerializeField] private float mouthPatternCooldown = 10.0f;
    private float mouthTime = 0.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (!bossDefeated)
            {
                base.Update();

                laserTime += Time.deltaTime;
                if (laserTime >= laserPatternCooldown)
                {
                    float _rand = Random.Range(0.0f, 1.0f);
                    if (_rand < 0.45f)
                    {
                        laserSegment.LaserHoldPattern();
                    }
                    else if (_rand < 0.8f)
                    {
                        laserSegment.LaserSweepPattern();
                    }
                    else
                    {
                        laserSegment.LaserRepeatingPattern();
                    }
                    laserTime = 0.0f;
                }

                mouthTime += Time.deltaTime;
                if (mouthTime >= mouthPatternCooldown)
                {
                    float _rand = Random.Range(0.0f, 1.0f);
                    if (_rand < 0.5f)
                    {
                        mouthSegment.MouthStreamPattern();
                    }
                    else
                    {
                        mouthSegment.MouthSprayPattern();
                    }
                    mouthTime = 0.0f;
                }

            }
            else
            {
                //if (spawnParticles.isPlaying)
                //    spawnParticles.Stop();
            }
        }
    }

    public override void TakeDamage(float _damage)
    {
        if (IsInvoking("revertColour"))
        {
            CancelInvoke("revertColour");
        }
        bossRenderer.color = Color.red;
        Invoke("revertColour", .1f);
        
        base.TakeDamage(_damage);
    }
    
    protected override IEnumerator bossDeath()
    {
        Explosion ex;

        #region mouth segment explosion
        ex = ExplosionManager.instance.PoolingExplosion(mouthSegment.transform, 1);
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(.25f);

        ex = ExplosionManager.instance.PoolingExplosion(mouthSegment.transform, 1);
        ex.transform.position = Vector3.Lerp(mouthSegment.transform.position, mouthSegment.previousSegment.transform.position, 0.5f);
        ex.gameObject.SetActive(true);
        ex.explode();
        mouthSegment.gameObject.SetActive(false);
        yield return new WaitForSeconds(.25f);

        ex = ExplosionManager.instance.PoolingExplosion(mouthSegment.previousSegment.transform, 1);
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(.25f);
        ex = ExplosionManager.instance.PoolingExplosion(mouthSegment.previousSegment.transform, 1);
        ex.transform.position = Vector3.Lerp(mouthSegment.previousSegment.transform.position, mouthSegment.previousSegment.previousSegment.transform.position, 0.5f);
        ex.gameObject.SetActive(true);
        ex.explode();
        mouthSegment.previousSegment.gameObject.SetActive(false);
        yield return new WaitForSeconds(.25f);

        ex = ExplosionManager.instance.PoolingExplosion(mouthSegment.previousSegment.previousSegment.transform, 1);
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(.25f);
        ex = ExplosionManager.instance.PoolingExplosion(mouthSegment.previousSegment.previousSegment.transform, 1);
        ex.transform.position = Vector3.Lerp(mouthSegment.previousSegment.previousSegment.transform.position, transform.position, 0.3f);
        ex.gameObject.SetActive(true);
        ex.explode();
        mouthSegment.previousSegment.previousSegment.gameObject.SetActive(false);
        yield return new WaitForSeconds(.25f);
        #endregion

        #region laser segment
        ex = ExplosionManager.instance.PoolingExplosion(laserSegment.transform, 1);
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(.25f);
        ex = ExplosionManager.instance.PoolingExplosion(laserSegment.transform, 1);
        ex.transform.position = Vector3.Lerp(laserSegment.transform.position, laserSegment.previousSegment.transform.position, 0.5f);
        ex.gameObject.SetActive(true);
        ex.explode();
        laserSegment.gameObject.SetActive(false);
        yield return new WaitForSeconds(.25f);

        ex = ExplosionManager.instance.PoolingExplosion(laserSegment.previousSegment.transform, 1);
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(.25f);
        ex = ExplosionManager.instance.PoolingExplosion(laserSegment.previousSegment.transform, 1);
        ex.transform.position = Vector3.Lerp(laserSegment.previousSegment.transform.position, laserSegment.previousSegment.previousSegment.transform.position, 0.5f);
        ex.gameObject.SetActive(true);
        ex.explode();
        laserSegment.previousSegment.gameObject.SetActive(false);
        yield return new WaitForSeconds(.25f);

        ex = ExplosionManager.instance.PoolingExplosion(laserSegment.previousSegment.previousSegment.transform, 1);
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(.25f);
        ex = ExplosionManager.instance.PoolingExplosion(laserSegment.previousSegment.previousSegment.transform, 1);
        ex.transform.position = Vector3.Lerp(laserSegment.previousSegment.previousSegment.transform.position,transform.position, 0.3f);
        ex.gameObject.SetActive(true);
        ex.explode();
        laserSegment.previousSegment.previousSegment.gameObject.SetActive(false);
        yield return new WaitForSeconds(.25f);
        #endregion

        ex = ExplosionManager.instance.PoolingExplosion(transform, 2);
        ex.gameObject.SetActive(true);
        ex.explode();
        bossRenderer.enabled = false;

        //EnemyManager.instance.NextLevel();
        StartCoroutine(base.bossDeath());
        
        yield return new WaitForSeconds(0.1f);
    }

    protected override void Movement()
    {
        if (!atStartPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, 2 * Time.deltaTime);
            if (Vector2.SqrMagnitude(transform.position - startPosition) < 0.1f)
            {
                atStartPosition = true;
            }
        }
    }

    protected override void Death()
    {
        laserSegment.Reset();
        mouthSegment.Reset();
        base.Death();
    }
}
