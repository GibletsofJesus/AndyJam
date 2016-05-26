using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
    private float health;
    Projectile projectile;
    private float speed;
    public GameObject shootTransform;

    void Awake()
    {
        projectile = GetComponent<Projectile>();
    }
   
	// Use this for initialization
	void Start () {
	
	}
   
	// Update is called once per frame
	void Update ()
    {
      
        Death();
	}
   
    public virtual void Shoot()
    {
        Projectile p = ProjectileManager.instance.PoolingProjectile();
        p.SetProjectile(10, transform.up);
        p.transform.position = shootTransform.transform.position;
        p.gameObject.SetActive(true);
       // p.transform.position = transform.position;

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
    
    public void SetActor(float _health, float _speed)
    {
        speed = _speed;
        health = _health;
    }
}
