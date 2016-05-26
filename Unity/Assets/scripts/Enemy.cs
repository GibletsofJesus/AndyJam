using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    RaycastHit2D hit;
    private float health;
    Projectile projectile;
    float speed;
    void Awake()
    {
        projectile = GetComponent<Projectile>();
    }

    void Update()
    {
        Movement();
    }
    public void SetEnemy(float _health,float _speed)
    {
        speed = _speed;
        health = _health;
    }
    void Shoot()
    {
       hit = Physics2D.Raycast(transform.position + transform.up, transform.up, 10);
       if (hit.rigidbody.gameObject.tag == "Player")
       {
           projectile.Fire(transform.up);
       }

    }
    void Movement()
    {
        Vector2 movement = transform.up * (speed * Time.deltaTime);
        transform.Translate(movement, Space.World);
    }

}
