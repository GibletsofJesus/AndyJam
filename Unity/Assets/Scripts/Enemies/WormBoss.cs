using UnityEngine;
using System.Collections;

public class WormBoss : Boss
{
    [SerializeField] private Transform[] rightPivots = null;
    [SerializeField] private Transform[] leftPivots = null;

    [SerializeField] private LaserModule laser = null;
    [SerializeField] private Transform mouth = null;

    [SerializeField] private SpriteRenderer bossRenderer = null;
    // [SerializeField] private ParticleSystem spawnParticles;

    private bool atStartPosition = false;
    [SerializeField] private Vector3 startPosition = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
    }

    //void SpawnBossEnemies(Vector2 _spawnPoint, Enemy _enemy)
    //{
    //    Enemy e = EnemyManager.instance.EnemyPooling(_enemy);
    //    e.GetComponent<Enemy_Circle>().bossMode = true;
    //    e.transform.position = _spawnPoint;
    //    e.gameObject.SetActive(true);
    //    e.hideFlags = HideFlags.HideInHierarchy;
    //}

    //protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
    //{
    //    if (!bossDefeated)
    //    {
    //        if (shootCooldown >= shootRate)
    //        {
    //            for (int i = 0; i < _shootTransform.Length; i++)
    //            {
    //                Vector3 shootDir = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y) - _shootTransform[i].transform.position;
    //                if (shootDir.x > 0)
    //                    shootDir.x = Mathf.Clamp(shootDir.x, 0, 7.5f);
    //                else
    //                    shootDir.x = Mathf.Clamp(shootDir.x, -7.5f, 0);

    //                Projectile p = ProjectileManager.instance.PoolingProjectile(_shootTransform[i].transform);
    //                p.SetProjectile(_projData, shootDir);
    //                p.transform.position = _shootTransform[i].transform.position;
    //                p.gameObject.SetActive(true);
    //                shootCooldown = 0;
    //                p.GetComponentInChildren<ParticleSystem>().startLifetime = .1f;
    //            }
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    // Update is called once per frame
    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (!bossDefeated)
            {
                base.Update();
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
        bossRenderer.color = Color.red;
        Invoke("revertColour", .1f);
        
        base.TakeDamage(_damage);
    }

    public void doDeath()
    {
        StartCoroutine(bossDeath());
    }

    protected override IEnumerator bossDeath()
    {
        Player.instance.IncreaseScore(score);
        EnemyManager.instance.NextLevel();

        //ex = ExplosionManager.instance.PoolingExplosion(mouthShot.transform, 2);
        //ex.gameObject.SetActive(true);
        //ex.explode();
        //bossRenderer.enabled = false;

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
}
