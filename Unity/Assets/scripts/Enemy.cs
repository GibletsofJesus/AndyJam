using UnityEngine;
using System.Collections;

public class Enemy : Actor
{
    RaycastHit2D hit;
   
  

    void Update()
    {
        Movement();
    }
   
    
    
      
    public override void Shoot()
    {
        hit = Physics2D.Raycast(transform.position + transform.up, transform.up, 10);
        if (hit.rigidbody.gameObject.tag == "Player")
        {
            base.Shoot();
        }
    }
    void Movement()
    {
        Vector2 movement = transform.up * GetSpeed();
        transform.Translate(movement, Space.World);
    }
   
}
