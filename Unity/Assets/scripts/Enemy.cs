using UnityEngine;
using System.Collections;

public class Enemy : Actor
{
   
    Ray2D rayCast;
    RaycastHit2D hit;  
    void Start()
    {
        SetActor(20, 1);
    }
    void Update()
    {
     //   Movement();
       
        rayCast.origin = gameObject.transform.position + -gameObject.transform.up;
        rayCast.direction = -transform.up * 10;
        hit = Physics2D.Raycast(rayCast.origin, rayCast.direction * 10);
        Debug.DrawRay(rayCast.origin, rayCast.direction * 10, Color.red);
        if (hit.collider)
        {
            Debug.Log(hit.collider.gameObject.ToString());
            if (hit.rigidbody.gameObject.GetComponent<Actor>())
            {
                Debug.Log("hit");
                //if (Vector2.Distance(transform.position, hit.point) > 5.5f)

                Shoot();
            }
        }
       
    }
   
    
    
      
    public override void Shoot()
    {
                      Debug.Log("hit player");
                base.Shoot();
          
       
    }
    void Movement()
    {
        Vector2 movement = -transform.up * GetSpeed();
        transform.Translate(movement, Space.World);
    }
   
}
