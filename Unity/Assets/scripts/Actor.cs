using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
    private float health;
    public Projectile projectile;
    private float speed;
    public GameObject shootTransform;
    float shotCooldown;
    float maxShotCooldown;

    void Awake()
    {
        shotCooldown = maxShotCooldown;
    }
   
	// Use this for initialization
	void Start () {
	
	}
   
	// Update is called once per frame
	public virtual void Update ()
    {
      
        Death();
        CoolDown();
	}
   
    public virtual void Shoot(Vector2 _direction)
    {
        if (shotCooldown >= maxShotCooldown)
        {
            Projectile p = ProjectileManager.instance.PoolingProjectile();
            p.SetProjectile(10, _direction,gameObject);
            p.transform.position = shootTransform.transform.position;
            p.gameObject.SetActive(true);
            // p.transform.position = transform.position;
        }

    }
   void CoolDown()
    {
       if (shotCooldown<maxShotCooldown)
       {
           shotCooldown += Time.deltaTime;
       }
    }
    public void TakeDamage(float _damage)
    {
        health -= _damage;
    }
   
    void Death()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public float GetSpeed()
    {
        return speed * Time.deltaTime; ;
    }
    
    public void SetActor(float _health, float _speed,float _maxShotCooldown)
    {
        speed = _speed;
        health = _health;
        maxShotCooldown = _maxShotCooldown;
    }
}
