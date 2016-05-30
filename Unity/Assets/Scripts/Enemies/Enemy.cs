using UnityEngine;
using System.Collections;

public class Enemy : Actor
{
   
    Ray2D rayCast;
    RaycastHit2D hit;
    public Vector2 target;
    protected float safeHealth;
    public Rigidbody2D body;
        
    void Start()
    {
       
        SetActor(2,1, 1.5f,0.8f);
        safeHealth = GetHealth();
    }
   public override void Update()
    {
        base.Update();
   
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
       
        if (ShotCoolDown())
        {
            Shoot(-transform.up.normalized, shootTransform,gameObject.tag,false);
        }
       Movement();
       KillEnemy();
             
    }
   
    public virtual void ResetEnemy()
   {
       ResetHealth(safeHealth);
   }
    void OnCollisionEnter2D(Collision2D col)
   {
        if (col.gameObject.tag == gameObject.tag)
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
        else if (col.gameObject.GetComponent<playerMovement>())
        {
            col.gameObject.GetComponent<playerMovement>().TakeDamage(GetDamage());
            gameObject.SetActive(false);
        }
   }

    public virtual void Movement()
    {
        Vector2 movement = -transform.up * GetSpeed();
        transform.Translate(movement, Space.World);
    }
    public void KillEnemy()
    {
        Vector3 posToCam = Camera.main.WorldToViewportPoint(transform.position);
        if (posToCam.y<-0.1f)
        {
            gameObject.SetActive(false);
        }
    }
   
}
