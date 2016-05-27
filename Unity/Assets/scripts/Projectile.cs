using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    int damage;
    float maxAlive;
    float aliveTime = 1.5f;
    float speed;
    Vector2 direction;
    Rigidbody2D body;
    string ignoreActor;
    public TrailRenderer trail;
    Vector3 screenBottom;
    Vector3 screenTop;
    // Use this for initialization
    public virtual void Start()
    {
        screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        screenTop = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 1));
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
        OffScreen();
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
      void OffScreen()
    {
          if (transform.position.y>screenTop.y||transform.position.y<screenBottom.y)
          {
              DeactivateProj();
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
