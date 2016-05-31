using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionAoe : MonoBehaviour 
{
	[SerializeField] private CircleCollider2D col = null;

	public delegate void ExplosionDelegate();
	public ExplosionDelegate deactivateProj = null;
	
	private bool explosionActive = false;
	private int explosionFrame = 0;
	private string explosionTag = string.Empty;
	private float damage = 10.0f;

	//Easiest way to get triggers to work appropriately
	private void FixedUpdate()
	{
		explosionFrame += 1;
		if (explosionFrame > 2) 
		{
			Reset ();
		}
	}
	
	private void OnTriggerStay2D(Collider2D _col)
	{
		if(explosionActive)
		{
			if(explosionTag == _col.tag)
			{
				_col.GetComponent<Actor>().TakeDamage(damage);
			}
		}
	}
	
	public void ActivateExplosion(string _find = "", float _damage = 10.0f, float _radius = 10.0f)
	{
		gameObject.SetActive (true);
		explosionActive = true;
		explosionFrame = 0;
		explosionTag = _find;
		col.radius = _radius;
		damage = _damage;
	}
	
	public void Reset()
	{
		deactivateProj ();
		gameObject.SetActive (false);
		explosionActive = false;
		explosionTag = string.Empty;
	}
}
	
