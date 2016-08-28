using UnityEngine;
using System.Collections;

public class PlayerBackup : Actor
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] public AudioClip[] shootSounds;
    [SerializeField] public ParticleSystem[] muzzleflash;
  
    protected override void Awake()
    {
        base.Awake();
    }
    
    public void HomingProjectiles(bool _homing, float _radius)
    {
        base.projData.homingBullets = _homing;
        base.projData.homingRadius = _radius;
    }

    void Start()
    {
        if (GreenShip.instance)
        {
            spriteRenderer.sprite = GreenShip.instance.ship;
        }
    }
    
    protected void FixedUpdate()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            transform.rotation = Player.instance.transform.rotation;
        }
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public override void TakeDamage(float _damage)
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.GameOver)
        {
            _damage = 0;
        }

        soundManager.instance.playSound(0);
        health -= _damage;
        if (health <= 0)
        {
            Death();
            Reset();
        }
        else
        {
            if (IsInvoking("revertColour"))
            {
                CancelInvoke("revertColour");
            }
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("revertColour", .1f);
        }
    }

    public void Shoot()
    {
        for (int i = 0; i < shootTransform.Length; i++)
        {
            Projectile _p = ProjectileManager.instance.PoolingProjectile(shootTransform[i].transform);
            _p.SetProjectile(projData, Vector2.up);
            _p.transform.position = shootTransform[i].transform.position;
            _p.gameObject.SetActive(true);
            _p.GetComponentInChildren<ParticleSystem>().startLifetime = .2f;
        }

        foreach (ParticleSystem ps in muzzleflash)
        {
            ps.Emit(1);
        }

        soundManager.instance.playSound(shootSounds[Random.Range(0, shootSounds.Length)]);
    }

    protected override void Death()
    {
        Explosion ex = ExplosionManager.instance.PoolingExplosion(transform, 1);
        ex.transform.position = transform.position;
        gameObject.SetActive(false);
        ex.gameObject.SetActive(true);
        ex.explode();
    }

    public override void Reset()
    {
        base.Reset();
    }
  
}
