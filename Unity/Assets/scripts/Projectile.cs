using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    int damage;
    float maxAlive;
    float aliveTime = 2;
    float speed;
    Vector2 direction;
    Rigidbody2D body;
    string ignoreActor;
    public TrailRenderer trail;

    // Use this for initialization
    public virtual void Start()
    {
       
        body = GetComponent<Rigidbody2D>();
    }
  
    void Alive()
    {
        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0)
        {
            DeactivateProj();
        }
    }
    // Update is called once per frame
    public virtual void Update()
    {

        body.velocity = direction * (speed);// * Time.deltaTime);
        Alive();
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != ignoreActor)
        {
            if (col.gameObject.GetComponent<Actor>())
            {
                col.gameObject.GetComponent<Actor>().TakeDamage(damage);
                DeactivateProj();
            }
        }
    }    
      
    
    void DeactivateProj()
    {
        trail.time = 0.00001f;
        aliveTime = maxAlive;
        gameObject.SetActive(false);
        gameObject.GetComponent<Projectile>().enabled = false;
        
    }
    public void SetProjectile(int _damage, Vector2 _direction,string _ignoreActor,float _speed = 50
        , float _aliveTime = 2, float trailRendTime = 0.1f)
    {
        damage = _damage;
        direction = _direction;
        maxAlive = _aliveTime;
        speed = _speed;
        transform.up = _direction;
        ignoreActor = _ignoreActor;
        trail.time = trailRendTime;
    }
}
