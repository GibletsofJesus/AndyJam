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
	    SetActor(20, 1, 1, 0.8f);
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

        Vector2 lerp = Vector2.Lerp(transform.position, new Vector2(target.x, -transform.up.y), GetSpeed());

        if (lerp.y < target.y + 8)
        {
            withinDist = true;
        }
        if (withinDist)
        {
            lerp.y = target.y + 8;
            lerp.x = target.x;
        }

        transform.position = lerp;
    }
    public override void ResetEnemy()
    {
        base.ResetEnemy();
        withinDist = false;
    }
}
