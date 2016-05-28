using UnityEngine;
using System.Collections;

public class Enemy_Circle : Enemy 
{
    GameObject player;
    Vector2 moveTarget;
    bool circleJerks = false;
 
    // Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SetActor(10, 0, 5, 1);
        safeHealth = GetHealth();
	}
	
	// Update is called once per frame
	public override void Update () 
    {
       base.Update();
	}
    public override void Movement()
    {
        if (player != null)
        {
            moveTarget = player.transform.position;
        }
        Vector2 pos = transform.position;
        float distance = Vector2.Distance(transform.position, moveTarget);
        if (!circleJerks)
        {
            if (distance > 7)
            {
                transform.position = Vector2.MoveTowards(transform.position, moveTarget, GetSpeed() * 2);
            }
            else
            {
                    circleJerks = true;
            }
        }
        if (circleJerks)
        {
              transform.RotateAround(moveTarget, transform.forward, 45 * GetSpeed());
            
            if (distance<=6)
            {
                transform.position = Vector2.MoveTowards(transform.position, (pos + moveTarget), GetSpeed() * 2);
            }
            else if (distance>7)
            {
                transform.position = Vector2.MoveTowards(transform.position, moveTarget, GetSpeed() * 2);
            }
        }
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        circleJerks = false;
    }
    //public override void Shoot(Vector2 _direction, GameObject[] _shootTransform, string _ignore)
    //{
    //    if (transform.position.y > player.transform.position.y)
    //    {
    //        base.Shoot(_direction, _shootTransform, _ignore);
    //    }
    //}
}
