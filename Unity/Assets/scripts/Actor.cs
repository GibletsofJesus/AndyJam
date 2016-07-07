using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
	[SerializeField] protected float defaultHealth = 100.0f;
	protected float health;
	//[SerializeField] protected float defaultProjDamage = 20.0f;
	//protected float projDamage;
	[SerializeField] protected float defaultShootRate = 0.1f;
	protected float shootRate;
	protected float shootCooldown = 0.0f;
	[SerializeField] protected float defaultSpeed = 5.0f;
	protected float speed;

	[SerializeField] protected ProjectileData projData;

	[SerializeField] protected GameObject[] shootTransform;

	public Rigidbody2D rig = null;
   
	protected virtual void Awake()
	{
		health = defaultHealth;
		projData.projDamage = projData.defaultProjDamage;
		shootRate = defaultShootRate;
		speed = defaultSpeed;
		projData.owner = this;
		projData.parentTag = tag;
	}

	// Update is called once per frame
	protected virtual void Update ()
    {
        //Death();
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            CoolDown();
        }
	}
   
    protected virtual bool Shoot(ProjectileData _projData, Vector2 _direction,GameObject[] _shootTransform)
    {
        if (shootCooldown >= shootRate)
        {
            for (int i = 0; i < _shootTransform.Length; i++)
            {
                Projectile p;
                if (gameObject.name == "trojan horse" || gameObject.name == "trojan horse(clone)")
                    p = ProjectileManager.instance.PoolingEnemyProjectile(_shootTransform[i].transform);
                else
                    p = ProjectileManager.instance.PoolingProjectile(_shootTransform[i].transform);

                p.SetProjectile(_projData, _direction);
                p.transform.position = _shootTransform[i].transform.position;
                p.gameObject.SetActive(true);
                shootCooldown = 0;
                if (tag == "Enemy")
                {
                    p.GetComponentInChildren<ParticleSystem>().startLifetime = .15f;
                }
                else
                {
                    p.GetComponentInChildren<ParticleSystem>().startLifetime = .2f;
                }
            }
            return true;
        }
        return false;
    }

    protected void CoolDown()
    {
		shootCooldown = (shootCooldown + Time.deltaTime) > shootRate ? shootRate : (shootCooldown + Time.deltaTime);
    }

    public virtual void TakeDamage(float _damage)
    {
        soundManager.instance.playSound(0);
        if (GetComponent<SpriteRenderer>() && tag == "Enemy")
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("revertColour", .1f);
        }
        health -= _damage;
		if(health <= 0)
        {
			Death ();
		}
    }

    protected void revertColour()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    protected virtual void Death()
    {
        Explosion ex = ExplosionManager.instance.PoolingExplosion(transform, 0);
        ex.transform.position = transform.position;
        Reset();
        gameObject.SetActive(false);
        ex.gameObject.SetActive(true);
        ex.explode();
    }

    public virtual void Reset()
    {
        shootCooldown = 0.0f;
		health = defaultHealth;
		projData.projDamage = projData.defaultProjDamage;
		shootRate = defaultShootRate;
		speed = defaultSpeed;
	}
}
