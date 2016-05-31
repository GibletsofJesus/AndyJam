using UnityEngine;
using System.Collections;

public class Enemy : Actor
{
 
    public float contactHitDamage = 5;
	[SerializeField] private int score = 100;
    Vector3 screenBottom;

    protected override void Awake()
	{
		base.Awake ();
        screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(.3f, 0f));
    }

    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();

            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);

            if (transform.position.y > screenBottom.y + 2)
            {
                if (!GetComponent<Enemy_Trojan>())
                    Shoot(projData, -transform.up.normalized, shootTransform);
                else
                {
                    Shoot(projData, -(transform.position - new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y)), shootTransform);
                }
            }

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
