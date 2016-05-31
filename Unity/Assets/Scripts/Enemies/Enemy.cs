using UnityEngine;
using System.Collections;

public class Enemy : Actor
{
 
    public float contactHitDamage = 5;
	[SerializeField] private int score = 100;
      
	protected override void Awake()
	{
		base.Awake ();
	}

    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();

            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);


            Shoot(projData, -transform.up.normalized, shootTransform);

            Movement();
            
            KillEnemy();
        }
    }

   protected virtual void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == "Player")
        {
            _col.gameObject.GetComponent<Actor>().TakeDamage(contactHitDamage);
            Death();
           // gameObject.SetActive(false);
        }
    }

    protected virtual void Movement()
    {
       
    }

    protected void KillEnemy()
    {
        Vector3 posToCam = Camera.main.WorldToViewportPoint(transform.position);
        if (posToCam.y<-0.1f)
        {
            gameObject.SetActive(false);
        }
    }

	protected override void Reset()
	{
		base.Reset ();
	}

    protected override void Death()
    {
        base.Death();
        Player.instance.IncreaseScore(score);
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
    } 
}
