using UnityEngine;
using System.Collections;

public class Enemy : Actor
{
 
    public float contactHitDamage = 5;
	[SerializeField] private int defaultScore = 100;
    Vector3 screenBottom;
    public int score;

    protected override void Awake()
	{
		base.Awake ();
        score = defaultScore;
        screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(.3f, 0f));
    }

    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();

//            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);

            if (transform.position.y > screenBottom.y + 2)
            {
                Shoot(projData, -transform.up.normalized, shootTransform);
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

	public override void Reset()
	{
		base.Reset ();
        score = defaultScore;
	}

    protected override void Death()
    {
        base.Death();
        Player.instance.IncreaseScore(score);
    }

    public virtual void Death(bool _noScore)
    {
        base.Death();
        if (!_noScore)
        { 
            Player.instance.IncreaseScore(score);
        }
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
    } 
    public virtual void OnSpawn()
    {
        
    }
    protected void SetStat(float mod)
    {
        score *= (int)mod;
        health *= mod;
        projData.projDamage *= mod;

    }
}
