using UnityEngine;
using System.Collections;

public class Enemy : Actor
{
   
    Ray2D rayCast;
    RaycastHit2D hit;
    Vector2 target;
    void Start()
    {
       
        SetActor(20, 1,0.5f);
    }
   public override void Update()
    {
        base.Update();
     //   Movement();
       
        rayCast.origin = gameObject.transform.position + -gameObject.transform.up;
        rayCast.direction = -transform.up * 10;
        hit = Physics2D.Raycast(rayCast.origin, rayCast.direction * 10);
        Debug.DrawRay(rayCast.origin, rayCast.direction * 10, Color.red);
        if (hit.collider)
        {
            if (hit.rigidbody.gameObject.GetComponent<Actor>())
            {
                //if (Vector2.Distance(transform.position, hit.point) > 5.5f)

                Shoot(-transform.up.normalized);
            }
        }
       
    }
   
    
    
      
    public override void Shoot(Vector2 _direction)
    {
                    
                base.Shoot(_direction);
          
       
    }
    void Movement()
    {
        Vector2 movement = -transform.up * GetSpeed();
        transform.Translate(movement, Space.World);

    }
   
}
