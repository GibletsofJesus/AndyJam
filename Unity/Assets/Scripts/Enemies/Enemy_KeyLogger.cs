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
	    //SetActor(20, 1, 1, 0.8f);
       // safeHealth = GetHealth();
	}
	
	// Update is called once per frame
	protected override void Update () 
    {
        if (GameStateManager.instance.state == GameStateManager.GameState.Gameplay)
        {
            base.Update();
        }
	}

    protected override void Movement()
    {
        target = player.transform.position;

		Vector2 lerp = Vector2.MoveTowards(transform.position, new Vector2(target.x, target.y), speed*10* Time.deltaTime);

        if (lerp.y < target.y + 8)
        {
            withinDist = true;
        }
        if (withinDist)
        {
            lerp.y = target.y + 8;
            lerp.x = target.x;
        }

		transform.position = Vector2.MoveTowards(transform.position,lerp,speed*10* Time.deltaTime);
    }
    protected override void Reset()
    {
        base.Reset();
        withinDist = false;
    }
}
