using UnityEngine;
using UnityEditor;
using System.Collections;

public class EyeBoss : Boss
{
    public Transform mouthShot;
    public Vector2 bossTarget = new Vector2(0, 9.3f);
    public Transform[] eyeShot;
    public Enemy littleBastards;
    public Enemy rareBastards;
    float maxCool = 5;
    float minicool = 0.5f;
    float minicooler = 0.5f;
    float cool;
    int dosCount = 0;
    bool move = false;

    [SerializeField] private Material deathMaterial = null;

    [SerializeField]
    private SpriteRenderer bossRenderer = null;
    [SerializeField]
    private ParticleSystem spawnParticles = null;

    protected override void Awake()
    {
        cool = maxCool;
        base.Awake();
    }

    void SpawnBossEnemies(Vector2 _spawnPoint, Enemy _enemy)
    {
        Enemy e = EnemyManager.instance.EnemyPooling(_enemy);
        e.GetComponent<Enemy_Circle>().bossMode = true;
        e.NoScore();
        e.OnSpawn();
        e.transform.position = _spawnPoint;
        e.gameObject.SetActive(true);
        e.hideFlags = HideFlags.HideInHierarchy;
    }

    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
    {
        if (!bossDefeated)
        {
            if (shootCooldown >= shootRate)
            {
                for (int i = 0; i < _shootTransform.Length; i++)
                {
                    Vector3 shootDir = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y) - _shootTransform[i].transform.position;
                    if (shootDir.x > 0)
                        shootDir.x = Mathf.Clamp(shootDir.x, 0, 7.5f);
                    else
                        shootDir.x = Mathf.Clamp(shootDir.x, -7.5f, 0);

                    Projectile p = ProjectileManager.instance.PoolingProjectile(_shootTransform[i].transform);
                    p.SetProjectile(_projData, shootDir);
                    p.transform.position = _shootTransform[i].transform.position;
                    p.gameObject.SetActive(true);
                    shootCooldown = 0;
                    p.GetComponentInChildren<ParticleSystem>().startLifetime = .1f;
                }
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            if (!bossDefeated)
            {
                MiniCool();

                if (cool <= maxCool)
                {
                    cool += Time.deltaTime;
                }

                if (move)
                {
                    EyeShooting();
                }

                base.Update();
            }
            else
            {
                if (spawnParticles.isPlaying)
                    spawnParticles.Stop();
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

    //public void doDeath()
    //{
    //    StartCoroutine(bossDeath());
    //}

    protected override IEnumerator bossDeath()
    {
        bossRenderer.material = deathMaterial;
        Material m = bossRenderer.material;
        float burnAmount=0;
        
        GetComponent<Animator>().SetBool("death",true);
        Explosion ex;

        //also do eyes and mouth transforms

        foreach (GameObject go in base.shootTransform)
        {
            if (Random.value > .5f)
                ex = ExplosionManager.instance.PoolingExplosion(go.transform, 0);
            else
                ex = ExplosionManager.instance.PoolingExplosion(go.transform, 1);
            
            ex.gameObject.SetActive(true);
            ex.explode();
            yield return new WaitForSeconds(Random.Range(0.4f, .8f));
            burnAmount += 0.05f;
            m.SetFloat("_Fade", burnAmount);
        }

        foreach (Transform t in eyeShot)
        {
            if (Random.value > .5f)
                ex = ExplosionManager.instance.PoolingExplosion(t.transform, 0);
            else
                ex = ExplosionManager.instance.PoolingExplosion(t.transform, 1);
            
            ex.gameObject.SetActive(true);
            ex.explode();
            yield return new WaitForSeconds(Random.Range(0.4f, 0.75f));
            burnAmount += 0.05f;
            m.SetFloat("_Fade", burnAmount);
        }

        yield return new WaitForSeconds(1);
        ex = ExplosionManager.instance.PoolingExplosion(mouthShot.transform, 2);
        ex.gameObject.SetActive(true);
        ex.explode();
        burnAmount += 0.05f;
        m.SetFloat("_Fade", burnAmount);

        GetComponent<Animator>().Play("boss_die_idle");
        yield return new WaitForSeconds(1.5f);

        EnemyManager.instance.NextLevel();

        ex = ExplosionManager.instance.PoolingExplosion(mouthShot.transform, 2);
        ex.gameObject.SetActive(true);
        ex.explode();
        bossRenderer.enabled = false;
    }
    
    protected override void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, bossTarget, speed * 2 * Time.deltaTime);
        if (Vector2.Distance(transform.position, bossTarget) < 1)
        {
            move = true;
        }
    }

    void EyeShooting()
    {
        if (dosCount < 30 && cool >= maxCool)
        {
            if (!spawnParticles.isPlaying)
                spawnParticles.Play();

            if (MiniCool())
            {
                SpawnBossEnemies(mouthShot.position, littleBastards);
                minicooler = 0;
                dosCount++;
            }
        }
        else
        {
            spawnParticles.Stop();
            dosCount = 0;
        }
        if (dosCount >= 10 && cool >= maxCool)
        {
            cool = 0;
        }
    }

    bool MiniCool()
    {
        if (minicooler <= minicool)
        {
            minicooler += Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }

}
//[CustomEditor(typeof(EyeBoss))]
//public class ObjectBuilderEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        EyeBoss myScript = (EyeBoss)target;
//        if (GUILayout.Button("Kill boss"))
//        {
//            myScript.doDeath();
//        }
//    }
//}