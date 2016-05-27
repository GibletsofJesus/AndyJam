using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
    private float health;
    private float damage;
    public Projectile projectile;
    private float speed;
    public GameObject[] shootTransform;
    public float shotCooldown;
    public float maxShotCooldown;

    void Awake()
    {
        shotCooldown = maxShotCooldown;
    }
   
	// Update is called once per frame
	public virtual void Update ()
    {
        Death();
        CoolDown();
	}
   
    public virtual void Shoot(Vector2 _direction,GameObject[] _shootTransform, string _ignore)
    {
        for (int i = 0; i < _shootTransform.Length; i++)
        {
            Projectile p = ProjectileManager.instance.PoolingProjectile();
            p.SetProjectile(10, _direction, _ignore);
            p.transform.position = _shootTransform[i].transform.position;
            p.gameObject.SetActive(true);
            shotCooldown = 0;
        }
           
    }
    public bool ShotCoolDown()
    {
        return shotCooldown >= maxShotCooldown ? true : false;
    }
    void CoolDown()
    {
       if (shotCooldown<maxShotCooldown)
       {
           shotCooldown += Time.deltaTime;
       }
    }
    public virtual void TakeDamage(float _damage)
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
   public float GetHealth()
    {
        return health;
    }
    public float GetDamage()
    {
        return damage;
    }
    public void SetActor(float _health,float _damage, float _speed,float _maxShotCooldown)
    {
        speed = _speed;
        health = _health;
        damage = _damage;
        maxShotCooldown = _maxShotCooldown;
    }
    public void ResetHealth(float _health)
    {
        health = _health;
    }
}
