﻿using UnityEngine;
using System.Collections;

public class Enemy : Actor
{
    Ray2D rayCast;
    RaycastHit2D hit;
    public Vector2 target;
    public float contactHitDamage = 5;
	[SerializeField] private int score = 100;
      
	protected override void Awake()
	{
		base.Awake ();
	}

    void Start()
    {
        //SetActor(200,1, 1.5f,0.8f);
        //safeHealth = GetHealth();
    }

    protected override void Update()
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();

            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);


            Shoot(projData, -transform.up.normalized, shootTransform, false);

            Movement();
            KillEnemy();
        }
    }


   protected virtual void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == gameObject.tag)
        {
            Physics2D.IgnoreCollision(_col.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
         //   Debug.Log("enemy hit " + _col.gameObject.name);
        }
        if (_col.gameObject.tag == "Player")
        {
            _col.gameObject.GetComponent<Player>().TakeDamage(contactHitDamage);
            gameObject.SetActive(false);
        }
    }

    protected virtual void Movement()
    {
        Vector2 movement = -transform.up * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
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
		base.Death ();
		Player.instance.IncreaseScore (score);
	}
   
}
