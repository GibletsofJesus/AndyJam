using UnityEngine;
using System.Collections;

public class Enemy_Spam : Enemy 
{
    float startX;
    float sideStep = 3;
	// Use this for initialization
	protected override void Awake () 
    {
        base.Awake();
        startX = transform.position.x;
	}
	
	// Update is called once per frame
	protected override void Update () 
    {
        base.Update();
	}
    protected override void Movement()
    {
        if (Vector2.Distance(transform.position, new Vector2(startX + sideStep, transform.position.y)) < 1.5f)
        {
            sideStep *= -1;
        }
        Vector2 movement = new Vector2(startX + sideStep, -transform.up.y) * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }
}
