using UnityEngine;
using System.Collections;

public class Enemy_KeyLogger : Enemy 
{
    GameObject player;
    bool withinDist = false;
	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
	    SetActor(2, 1, 1, 0.8f);
        safeHealth = GetHealth();
	}
	
	// Update is called once per frame
	public override void Update () 
    {
        base.Update();
	}
    public override void Movement()
    {
        target = player.transform.position;

        Vector2 lerp = Vector2.MoveTowards(transform.position, new Vector2(target.x, target.y), GetSpeed()*10);

        if (lerp.y < target.y + 8)
        {
            withinDist = true;
        }
        if (withinDist)
        {
            lerp.y = target.y + 8;
            lerp.x = target.x;
        }

        transform.position = Vector2.MoveTowards(transform.position,lerp,GetSpeed()*10);
    }
    public override void ResetEnemy()
    {
        base.ResetEnemy();
        withinDist = false;
    }
}
