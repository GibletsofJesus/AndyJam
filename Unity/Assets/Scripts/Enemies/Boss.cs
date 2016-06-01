using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Boss : Enemy 
{
    public Transform mouthShot;
    public Vector2 bossTarget = new Vector2(0,9.3f);
    public Transform[] eyeShot;
    public Enemy littleBastards;
    public Enemy rareBastards;
    float maxCool = 5;
    float minicool = 0.5f;
    float minicooler = 0.5f;
    float cool;
    int dosCount = 0;
    bool move = false;

	[SerializeField] private Text passwordText = null;
	private string password;
	private string currentPassword;
	[SerializeField] private string[] passwordSelections = null;
	private int charactersHidden;

	// Use this for initialization
	void Start ()
    {
        //SetActor(100, 1, 1, 0.8f);
        cool = maxCool;
        //safeHealth = health;
	
	}

    void SpawnBossEnemies(Vector2 _spawnPoint, Enemy _enemy)
    {
        Enemy e = EnemyManager.instance.EnemyPooling(_enemy);
        e.transform.position = _spawnPoint;
        e.gameObject.SetActive(true);

    }

    protected override bool Shoot(ProjectileData _projData, Vector2 _direction, GameObject[] _shootTransform)
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
        return false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
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


            //Debug.Log("count " + dosCount + " cool " + cool);

            base.Update();
        }
    }

    public override void TakeDamage(float _damage)
    {
        soundManager.instance.playSound(0);
        if (GetComponent<SpriteRenderer>() && tag == "Enemy")
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("revertColour", .1f);
        }
        health = Mathf.Max(0.0f, health - _damage);
        RevealWord();
    }

    private void RevealWord()
    {
        while ((health / defaultHealth) < ((float)charactersHidden / (float)password.Length))
        {
            int i = 0;
            while (true)
            {
                if (currentPassword[i] == '_')
                {
                    if (Random.Range(0, 3) == 0)
                    {
                        --charactersHidden;
                        char[] array = currentPassword.ToCharArray();
                        array[i] = password[i];
                        currentPassword = new string(array);
                        passwordText.text = currentPassword;
                        return;
                    }
                }
                ++i;
                if (i == currentPassword.Length)
                {
                    i = 0;
                }
            }
        }
    }

    public override void OnSpawn()
    {
        password = passwordSelections[Random.Range(0, passwordSelections.Length - 1)];
        charactersHidden = password.Length;
        for (int i = 0; i < password.Length; ++i)
        {
            currentPassword += '_';
        }
        passwordText.text = currentPassword;
        BossWord.instance.BossActive(this, password);
    }

    public void PasswordEntered()
    {
        Death();
    }

    protected override void Reset()
    {
        currentPassword = string.Empty;
        base.Reset();
    }

    protected override void Death()
    {
        //do explosions
        StartCoroutine(bossDeath());
        Player.instance.IncreaseScore(base.score);

    }

    IEnumerator bossDeath()
    {
        Explosion ex = ExplosionManager.instance.PoolingExplosion(base.shootTransform[0].transform, 0);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(0.7f);

        ex = ExplosionManager.instance.PoolingExplosion(base.shootTransform[3].transform, 0);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(0.4f);

        ex = ExplosionManager.instance.PoolingExplosion(eyeShot[1], 1);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(0.2f);

        ex = ExplosionManager.instance.PoolingExplosion(base.shootTransform[1].transform, 0);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(0.35f);
        
        ex = ExplosionManager.instance.PoolingExplosion(eyeShot[2], 1);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(0.1f);

        ex = ExplosionManager.instance.PoolingExplosion(base.shootTransform[5].transform, 0);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(0.4f);

        ex = ExplosionManager.instance.PoolingExplosion(base.shootTransform[2].transform, 0);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(0.1f);

        ex = ExplosionManager.instance.PoolingExplosion(base.shootTransform[4].transform, 0);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(0.4f);

        ex = ExplosionManager.instance.PoolingExplosion(eyeShot[0], 1);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
        ex.explode();
        yield return new WaitForSeconds(2f);

        ex = ExplosionManager.instance.PoolingExplosion(mouthShot, 2);
        ex.transform.position = transform.position;
        ex.gameObject.SetActive(true);
		ex.explode();
		yield return new WaitForSeconds(.7f);
		base.Death ();
		GameStateManager.instance.WinState ();
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
            if (MiniCool())
            {
                SpawnBossEnemies(eyeShot[Random.Range(0, eyeShot.Length - 1)].position, littleBastards);
                minicooler = 0;
                dosCount++;
            }
        }
        else
        {
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