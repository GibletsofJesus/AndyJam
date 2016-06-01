using UnityEngine;
using System.Collections;

public class Enemy_Spam : Enemy 
{
    float startX;//=0;
    float sideStep = 5;
    float dist;
	// Use this for initialization
    void Start()
    {
      //  startX = transform.position.x;
    }
    protected override void Awake()
    {
        base.Awake();
    }
	
	// Update is called once per frame
	protected override void Update () 
    {
        base.Update();
        dist = Vector2.Distance(transform.position, new Vector2(startX + sideStep, transform.position.y));

	}
    protected override void Movement()
    {
        if (dist< 1.5f)
        {
            sideStep *= -1;
        }
        Vector2 movement = new Vector2((startX + sideStep), transform.position.y + -transform.up.y);
        transform.position = Vector2.MoveTowards(transform.position,movement,speed*Time.deltaTime);
    }
    public override void OnSpawn()
    {
       startX = transform.position.x;
    }
}
