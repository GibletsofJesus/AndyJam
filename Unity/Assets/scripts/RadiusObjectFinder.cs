﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadiusObjectFinder : MonoBehaviour 
{
	[SerializeField] private CircleCollider2D col = null;

	private bool finderActive = false;
	private string findTag = string.Empty;
	private float defaultRadius = 10.0f;

	private GameObject closestObject = null;

	//private float pingRate = 1.0f;
	//private float pingCooldown = 0.0f;

	private bool targetAcquired = false;

	private void Update()
	{
		/*if(pingCooldown > pingRate)
		{
			pingCooldown = 0.0f;
		}
		pingCooldown += Time.deltaTime;*/
		if (closestObject)
		{
			targetAcquired = (transform.position - closestObject.transform.position).sqrMagnitude <= (col.radius * col.radius);
			if(targetAcquired == false)
			{
				closestObject = null;
				targetAcquired = false;
			}
			else if (!closestObject.activeSelf) 
			{
				closestObject = null;
				targetAcquired = false;
			}
		}
	}

	private void OnTriggerStay2D(Collider2D _col)
	{
		if(finderActive)
		{
			if(!targetAcquired)
			{
				if(_col.tag == findTag)
				{
					if(!closestObject)
					{
						closestObject = _col.gameObject;
					}
					else
					{
						if(Vector3.Distance(_col.transform.position, transform.position) < Vector3.Distance(closestObject.transform.position, transform.position))
						{
							closestObject = _col.gameObject;
						}
					}
				}
			}
		}
	}

	public void ActivateFinder(string _find = "", float _radius = 10.0f)
	{
		gameObject.SetActive (true);
		finderActive = true;
		findTag = _find;
		col.radius = _radius;
		//pingCooldown = pingRate;
	}

	public void Reset()
	{
		gameObject.SetActive (false);
		finderActive = false;
		findTag = string.Empty;
		closestObject = null;
		targetAcquired = false;
		col.radius = defaultRadius;
	}

	public GameObject GetClosestObject()
	{
		return closestObject;
	}



}